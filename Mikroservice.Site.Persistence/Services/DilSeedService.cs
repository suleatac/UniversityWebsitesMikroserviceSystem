using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.Services;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.SeedDatas;
using System.Data;

namespace Mikroservice.Site.Persistence.Services
{
    public class DilSeedService : ISeedService
    {
        private readonly IDilRepository _DilRepository;
        private readonly ILogger<DilSeedService> _logger;
        private readonly IUnitOfWork _UnitOfWork;

        public DilSeedService(
            IDilRepository DilRepository,
            IUnitOfWork UnitOfWork,
            ILogger<DilSeedService> logger)
        {
            _DilRepository = DilRepository;
            _UnitOfWork = UnitOfWork;
            _logger = logger;
        }

        public int Sira => 5;

        public async Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default)
        {
            // En az 1 dil var mı kontrol et
            var dilVarMı = await _DilRepository.AnyAsync(cancellationToken);
            _logger.LogDebug("Database'de seed edilen veri var mı?: {HasDil}", dilVarMı);
            return dilVarMı;
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
                    var Diller = DilSeedData.GetDilSeedDatas();

                    await ProcessDillerAsync(Diller, cancellationToken);

                    await _UnitOfWork.CommitAsync(cancellationToken);
                    _logger.LogInformation("Diller seed işlemi tamamlandı. İşlenen: {Count}", Diller.Count);

                }
                catch (Exception ex)
                {
                    await _UnitOfWork.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Senkronizasyon hatası");
                    throw;
                }
            });
        }

        private async Task ProcessDillerAsync(List<Dil> Diller, CancellationToken cancellationToken)
        {

            foreach (var Dil in Diller)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _DilRepository.AddAsync(Dil);

            }

            await _UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
