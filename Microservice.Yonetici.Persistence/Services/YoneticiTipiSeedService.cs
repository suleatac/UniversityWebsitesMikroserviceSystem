using AutoMapper;
using Microservice.Yonetici.Application.Contracts.IRepositories;
using Microservice.Yonetici.Application.Contracts.Services;
using Microservice.Yonetici.Domain.Entities;
using Microservice.Yonetici.Domain.SeedDatas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Microservice.Yonetici.Persistence.Services
{
    public class YoneticiTipiSeedService : IYoneticiTipiSeedService
    {

        private readonly IYoneticiTipiRepository _YoneticiTipiRepository;
        private readonly ILogger<YoneticiTipiSeedService> _logger;
        private readonly IUnitOfWork _UnitOfWork;
   
        public YoneticiTipiSeedService(
            IYoneticiTipiRepository YoneticiTipiRepository,
            IUnitOfWork UnitOfWork,
            ILogger<YoneticiTipiSeedService> logger)
        {
            _YoneticiTipiRepository = YoneticiTipiRepository;
            _UnitOfWork = UnitOfWork;
            _logger = logger;
        }
        public async Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default)
        {
            // En az 1 personel var mı kontrol et
            var yoneticiTipiVarMı = await _YoneticiTipiRepository.AnyAsync(cancellationToken);
            _logger.LogDebug("Database'de seed edilen veri var mı?: {HasYoneticiTipi}", yoneticiTipiVarMı);
            return yoneticiTipiVarMı;
        }

        public async Task SeedInitialDataAsync(CancellationToken cancellationToken = default)
        {
            // 🔥 CRITICAL: ExecutionStrategy al
            var strategy = _UnitOfWork.GetExecutionStrategy();

            await strategy.ExecuteAsync(async () => {
                await _UnitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

                try
                {
                    _logger.LogInformation("Yönetici tipleri seed işlemi başlatılıyor.");

                    var YoneticiTipleri = YoneticiTipiSeedData.GetYoneticiTipiSeedDatas();

                    await ProcessYoneticiTipleriAsync(YoneticiTipleri, cancellationToken);

                    await _UnitOfWork.CommitAsync(cancellationToken);
                    _logger.LogInformation("Yönetici tipleri seed işlemi tamamlandı. İşlenen: {Count}", YoneticiTipleri.Count);


                }
                catch (Exception ex)
                {
                    await _UnitOfWork.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Senkronizasyon hatası");
                    throw;
                }
            });
        }

        private async Task ProcessYoneticiTipleriAsync(List<YoneticiTipi> YöneticiTipleri, CancellationToken cancellationToken)
        {

            foreach (var YoneticiTipi in YöneticiTipleri)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _YoneticiTipiRepository.AddAsync(YoneticiTipi);

            }

            await _UnitOfWork.SaveChangesAsync(cancellationToken);
        }

    }
}
