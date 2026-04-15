using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.Services;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.SeedDatas;
using System.Data;

namespace Mikroservice.Site.Persistence.Services
{
    public class BirimSeedService:ISeedService
    {
        private readonly IBirimRepository _BirimRepository;
        private readonly ILogger<BirimSeedService> _logger;
        private readonly IUnitOfWork _UnitOfWork;

        public BirimSeedService(
            IBirimRepository BirimRepository,
            IUnitOfWork UnitOfWork,
            ILogger<BirimSeedService> logger)
        {
            _BirimRepository = BirimRepository;
            _UnitOfWork = UnitOfWork;
            _logger = logger;
        }

        public int Sira => 6;

        public async Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default)
        {
            // En az 1 birim var mı kontrol et
            var birimVarMı = await _BirimRepository.AnyAsync(cancellationToken);
            _logger.LogDebug("Database'de seed edilen veri var mı?: {HasBirim}", birimVarMı);
            return birimVarMı;
        }

        public async Task SeedInitialDataAsync(CancellationToken cancellationToken = default)
        {
            // 🔥 CRITICAL: ExecutionStrategy al
            var strategy = _UnitOfWork.GetExecutionStrategy();

            await strategy.ExecuteAsync(async () => {
                await _UnitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

                try
                {
                    _logger.LogInformation("Hedefler seed işlemi başlatılıyor.");
                    var Birimler = BirimSeedData.GetBirimSeedDatas();

                    await ProcessBirimlerAsync(Birimler, cancellationToken);

                    await _UnitOfWork.CommitAsync(cancellationToken);
                    _logger.LogInformation("Birimler seed işlemi tamamlandı. İşlenen: {Count}", Birimler.Count);

                }
                catch (Exception ex)
                {
                    await _UnitOfWork.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Senkronizasyon hatası");
                    throw;
                }
            });
        }

        private async Task ProcessBirimlerAsync(List<Birim> Birimler, CancellationToken cancellationToken)
        {

            foreach (var Birim in Birimler)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _BirimRepository.AddAsync(Birim);

            }

            await _UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
