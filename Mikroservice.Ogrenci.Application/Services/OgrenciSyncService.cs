using AutoMapper;
using Microservice.Ogrenci.Application.Contracts.IRepositories;
using Microservice.Ogrenci.Domain.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Ogrenci.Application.Contracts.DTOs;
using Mikroservice.Ogrenci.Application.Contracts.Services;
using Mikroservice.Ogrenci.Domain.Exceptions;
using System.Data;
using System.Text.Json;

namespace Mikroservice.Ogrenci.Application.Services
{
    public class OgrenciSyncService : IOgrenciSyncService
    {
        private readonly IOgrenciExternalApiService _externalApiService;
        private readonly IOgrenciRepository _ogrenciRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OgrenciSyncService> _logger;
        private readonly IMapper _mapper;
        public OgrenciSyncService(
            IOgrenciExternalApiService externalApiService,
            IOgrenciRepository ogrenciRepository,
            IUnitOfWork unitOfWork,
            ILogger<OgrenciSyncService> logger,
            IMapper mapper)
        {
            _externalApiService = externalApiService;
            _ogrenciRepository = ogrenciRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<OgrenciSyncResponse> SyncOgrencisAsync(
         DateTime? lastUpdateDate = null,
         CancellationToken cancellationToken = default)
        {
            // 🔥 CRITICAL: ExecutionStrategy al
            var strategy = _unitOfWork.GetExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await _unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

                try
                {
                    _logger.LogInformation("Öğrenci senkronizasyonu başlatılıyor. Tarih: {LastUpdateDate}",
                        lastUpdateDate?.ToString("yyyy-MM-dd"));

                    // var ogrenciler = OgrenciSeedData.GetOrnekOgrenciler();


                    var request = new OgrenciSyncRequest(
                     "GetPersonStudents",
                     lastUpdateDate.HasValue
                         ? new { SonGuncellemeTarihi = lastUpdateDate.Value.ToString("yyyy-MM-dd") }
                         : new { });

                    var apiResponse = await _externalApiService.GetOgrencisAsync(request, cancellationToken);

                    if (!apiResponse.IsSuccess)
                    {
                        _logger.LogWarning("External API hatası: {ErrorMessage}", apiResponse.ErrorMessage);
                        return new OgrenciSyncResponse(new List<Microservice.Ogrenci.Domain.Entities.Ogrenci>(), 0);
                    }

                    var ogrenciler = ParseOgrencis(apiResponse.RawContent);





                    await ProcessOgrencisAsync(ogrenciler, cancellationToken);

                    await _unitOfWork.CommitAsync(cancellationToken);

                    _logger.LogInformation("Senkronizasyon tamamlandı. İşlenen: {Count}", ogrenciler.Count);

                    return new OgrenciSyncResponse(ogrenciler, ogrenciler.Count);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Senkronizasyon hatası");
                    throw;
                }
            });
        }
        private List<Microservice.Ogrenci.Domain.Entities.Ogrenci> ParseOgrencis(string jsonContent)
        {
            if (jsonContent == "\"Access is denied.\"" ||
                jsonContent.Contains("maksimum çağrıla sayısına ulaştı"))
            {
                throw new OgrenciSyncException("External API erişim hatası");
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<Microservice.Ogrenci.Domain.Entities.Ogrenci>>(jsonContent, options) ?? [];
        }

        private async Task ProcessOgrencisAsync(List<Microservice.Ogrenci.Domain.Entities.Ogrenci> Ogrencis, CancellationToken cancellationToken)
        {
            var processed = 0;
            foreach (var Ogrenci in Ogrencis)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var existing = await _ogrenciRepository.GetOgrenciByOgrenciProgramId(Ogrenci.OgrenciProgramId);

                if (existing == null)
                    await _ogrenciRepository.AddAsync(Ogrenci);
                else
                {
                    _mapper.Map(Ogrenci, existing);
                    _ogrenciRepository.Update(existing);
                }

                processed++;
                if (processed % 100 == 0)
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

            }

            // Tüm değişiklikleri bir seferde kaydet
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}