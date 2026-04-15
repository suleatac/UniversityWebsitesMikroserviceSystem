using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.Services;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.SeedDatas;
using System.Data;

namespace Mikroservice.Site.Persistence.Services
{
    public class HedefSeedService : ISeedService
    {
        private readonly IHedefRepository _HedefRepository;
        private readonly ILogger<HedefSeedService> _logger;
        private readonly IUnitOfWork _UnitOfWork;

        public HedefSeedService(
            IHedefRepository HedefRepository,
            IUnitOfWork UnitOfWork,
            ILogger<HedefSeedService> logger)
        {
            _HedefRepository = HedefRepository ;
            _UnitOfWork = UnitOfWork;
            _logger = logger;
        }
        public int Sira => 4;
        public async Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default)
        {
            // En az 1 hedef var mı kontrol et
            var hedefVarMı = await _HedefRepository.AnyAsync(cancellationToken);
            _logger.LogDebug("Database'de seed edilen veri var mı?: {HasHedef}", hedefVarMı);
            return hedefVarMı;
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
                    var Hedefler = HedefSeedData.GetHedefSeedDatas();

                    await ProcessHedeflerAsync(Hedefler, cancellationToken);

                    await _UnitOfWork.CommitAsync(cancellationToken);
                    _logger.LogInformation("Hedefler seed işlemi tamamlandı. İşlenen: {Count}", Hedefler.Count);


                }
                catch (Exception ex)
                {
                    await _UnitOfWork.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Senkronizasyon hatası");
                    throw;
                }
            });
        }

        private async Task ProcessHedeflerAsync(List<Hedef> Hedefler, CancellationToken cancellationToken)
        {

            foreach (var Hedef in Hedefler)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _HedefRepository.AddAsync(Hedef);

            }

            await _UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
