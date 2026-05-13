using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.Services;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.SeedDatas;
using System.Data;

namespace Mikroservice.Site.Persistence.Services
{
    public class TemplateSeedService : ISeedService
    {
        private readonly ITemplateRepository _TemplateRepository;
        private readonly ILogger<TemplateSeedService> _logger;
        private readonly IUnitOfWork _UnitOfWork;

        public TemplateSeedService(
            ITemplateRepository TemplateRepository,
            IUnitOfWork UnitOfWork,
            ILogger<TemplateSeedService> logger)
        {
            _TemplateRepository = TemplateRepository;
            _UnitOfWork = UnitOfWork;
            _logger = logger;
        }
        public int Sira => 6;

        public async Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default)
        {
            // En az 1 template var mı kontrol et
            var templateVarMı = await _TemplateRepository.AnyAsync(cancellationToken);
            _logger.LogDebug("Database'de seed edilen veri var mı?: {HasTemplate}", templateVarMı);
            return templateVarMı;
        }

        public async Task SeedInitialDataAsync(CancellationToken cancellationToken = default)
        {
            // 🔥 CRITICAL: ExecutionStrategy al
            var strategy = _UnitOfWork.GetExecutionStrategy();

            await strategy.ExecuteAsync(async () => {
                await _UnitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

                try
                {
                    _logger.LogInformation("Template'ler seed işlemi başlatılıyor.");
                    var Templates = TemplateSeedData.GetTemplateSeedDatas();

                    await ProcessTemplatesAsync(Templates, cancellationToken);

                    await _UnitOfWork.CommitAsync(cancellationToken);
                    _logger.LogInformation("Template'ler seed işlemi tamamlandı. İşlenen: {Count}", Templates.Count);


                }
                catch (Exception ex)
                {
                    await _UnitOfWork.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Senkronizasyon hatası");
                    throw;
                }
            });
        }

        private async Task ProcessTemplatesAsync(List<Template> Templates, CancellationToken cancellationToken)
        {

            foreach (var Template in Templates)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _TemplateRepository.AddAsync(Template);

            }

            await _UnitOfWork.SaveChangesAsync(cancellationToken);
        }

    }
}
