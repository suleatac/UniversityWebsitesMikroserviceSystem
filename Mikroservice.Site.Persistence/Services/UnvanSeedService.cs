using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.Services;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.SeedDatas;
using System.Data;

namespace Mikroservice.Site.Persistence.Services
{
    public class UnvanSeedService : ISeedService
    {
        private readonly IUnvanRepository _UnvanRepository;
        private readonly ILogger<UnvanSeedService> _logger;
        private readonly IUnitOfWork _UnitOfWork;

        public UnvanSeedService(
            IUnvanRepository UnvanRepository,
            IUnitOfWork UnitOfWork,
            ILogger<UnvanSeedService> logger)
        {
            _UnvanRepository = UnvanRepository;
            _UnitOfWork = UnitOfWork;
            _logger = logger;
        }
        public int Sira => 3;
        public async Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default)
        {
            // En az 1 unvan var mı kontrol et
            var unvanVarMı = await _UnvanRepository.AnyAsync(cancellationToken);
            _logger.LogDebug("Database'de seed edilen veri var mı?: {HasUnvan}", unvanVarMı);
            return unvanVarMı;
        }

        public async Task SeedInitialDataAsync(CancellationToken cancellationToken = default)
        {
            // 🔥 CRITICAL: ExecutionStrategy al
            var strategy = _UnitOfWork.GetExecutionStrategy();

            await strategy.ExecuteAsync(async () => {
                await _UnitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

                try
                {
                    _logger.LogInformation("Unvanlar seed işlemi başlatılıyor.");
                    var Unvanlar = UnvanSeedData.GetUnvanSeedDatas();

                    await ProcessUnvanlarAsync(Unvanlar, cancellationToken);

                    await _UnitOfWork.CommitAsync(cancellationToken);
                    _logger.LogInformation("Unvanlar seed işlemi tamamlandı. İşlenen: {Count}", Unvanlar.Count);


                }
                catch (Exception ex)
                {
                    await _UnitOfWork.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Senkronizasyon hatası");
                    throw;
                }
            });
        }

        private async Task ProcessUnvanlarAsync(List<Unvan> Unvanlar, CancellationToken cancellationToken)
        {

            foreach (var Unvan in Unvanlar)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _UnvanRepository.AddAsync(Unvan);

            }

            await _UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
