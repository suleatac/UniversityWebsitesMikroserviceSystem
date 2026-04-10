using AutoMapper;
using Microservice.Personel.Application.Contracts.DTOs;
using Microservice.Personel.Application.Contracts.IRepositories;
using Microservice.Personel.Application.Contracts.Services;
using Microservice.Personel.Domain.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Personel.Domain.Exceptions;
using System.Data;
using System.Text.Json;

namespace Mikroservice.Personel.Application.Services
{
    public class PersonelSyncService : IPersonelSyncService
    {
     
        private readonly IPersonelRepository _PersonelRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PersonelSyncService> _logger;
        private readonly IMapper _mapper;
        public PersonelSyncService(
            IPersonelRepository PersonelRepository,
            IUnitOfWork unitOfWork,
            ILogger<PersonelSyncService> logger,
            IMapper mapper)
        {
        
            _PersonelRepository = PersonelRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PersonelSyncResponse> SyncPersonelsAsync(
      DateTime? lastUpdateDate = null,
      CancellationToken cancellationToken = default)
        {
            // 🔥 CRITICAL: ExecutionStrategy al
            var strategy = _unitOfWork.GetExecutionStrategy();

            return await strategy.ExecuteAsync(async () => {
                await _unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

                try
                {
                    _logger.LogInformation("Personel senkronizasyonu başlatılıyor. Tarih: {LastUpdateDate}",
                        lastUpdateDate?.ToString("yyyy-MM-dd"));

                    //var request = new PersonelSyncRequest(
                    //"GetWorkers",
                    //lastUpdateDate.HasValue
                    // ? new {
                    //     GetPersonEncryptedId = true,
                    //     SonGuncellemeTarihi = lastUpdateDate.Value.ToString("u")
                    // }
                    //: new {
                    //    GetPersonEncryptedId = true
                    //});

                    //var apiResponse = await _externalApiService.GetPersonelsAsync(request, cancellationToken);

                    //if (!apiResponse.IsSuccess)
                    //{
                    //    _logger.LogWarning("External API hatası: {ErrorMessage}", apiResponse.ErrorMessage);
                    //    return new PersonelSyncResponse(new List<Microservice.Personel.Domain.Entities.Personel>(), 0);
                    //}

                    //var Personeller = ParsePersonels(apiResponse.RawContent);

                    // Metod ile
                    var Personeller = PersonelSeedData.GetOrnekPersoneller();

                    await ProcessPersonelsAsync(Personeller, cancellationToken);

                    await _unitOfWork.CommitAsync(cancellationToken);
                    _logger.LogInformation("Senkronizasyon tamamlandı. İşlenen: {Count}", Personeller.Count);

                    return new PersonelSyncResponse(Personeller, Personeller.Count);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Senkronizasyon hatası");
                    throw;
                }
            });
        }
        private List<Microservice.Personel.Domain.Entities.Personel> ParsePersonels(string jsonContent)
        {
            if (jsonContent == "\"Access is denied.\"" ||
                jsonContent.Contains("maksimum çağrıla sayısına ulaştı"))
            {
                throw new PersonelSyncException("External API erişim hatası");
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<Microservice.Personel.Domain.Entities.Personel>>(jsonContent, options) ?? [];
        }

        private async Task ProcessPersonelsAsync(List<Microservice.Personel.Domain.Entities.Personel> Personels, CancellationToken cancellationToken)
        {
            var processed = 0;
            foreach (var Personel in Personels)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var existing = await _PersonelRepository.GetPersonelByUsername(Personel.Username!);

                if (existing == null)
                {
                    await _PersonelRepository.AddAsync(Personel);
                }
                else
                {
                    _mapper.Map(Personel, existing);
                    _PersonelRepository.Update(existing);
                }

                processed++;
                if (processed % 100 == 0)
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}