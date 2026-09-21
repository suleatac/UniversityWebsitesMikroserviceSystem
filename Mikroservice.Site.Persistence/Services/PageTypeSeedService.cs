using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.Services;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.Enums;
using Mikroservice.Site.Domain.SeedDatas;
using System.Data;

namespace Mikroservice.Site.Persistence.Services
{
    /// <summary>
    /// Microservice.Web / Views / Templates / Template1 altindaki view'lara
    /// karsilik gelen PageType kayitlarini, TemplateSeedService ile olusturulan
    /// her template ve DilSeedService ile olusturulan her dil icin seed eder.
    ///
    /// TemplateSeedService'ye bagimlidir: Sira = 7 (TemplateSeedService Sira = 6),
    /// boylece seed siralamasi once template'leri, sonra bu sayfari uretir.
    /// </summary>
    public class PageTypeSeedService : ISeedService
    {
        private readonly IPageTypeRepository _pageTypeRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IDilRepository _dilRepository;
        private readonly ILogger<PageTypeSeedService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public PageTypeSeedService(
            IPageTypeRepository pageTypeRepository,
            ITemplateRepository templateRepository,
            IDilRepository dilRepository,
            IUnitOfWork unitOfWork,
            ILogger<PageTypeSeedService> logger)
        {
            _pageTypeRepository = pageTypeRepository;
            _templateRepository = templateRepository;
            _dilRepository = dilRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // Template (Sira 6) ve Dil (Sira 5) seed'lerinden SONRA calismali.
        public int Sira => 7;

        public async Task<bool> IsDatabaseSeededAsync(CancellationToken cancellationToken = default)
        {
            var pageTypeVarMı = await _pageTypeRepository.GetAll().AnyAsync(cancellationToken);
            _logger.LogDebug("Database'de seed edilen PageType var mı?: {HasPageType}", pageTypeVarMı);
            return pageTypeVarMı;
        }

        public async Task SeedInitialDataAsync(CancellationToken cancellationToken = default)
        {
            // 🔥 CRITICAL: ExecutionStrategy al
            var strategy = _unitOfWork.GetExecutionStrategy();

            await strategy.ExecuteAsync(async () => {
                await _unitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

                try
                {
                    _logger.LogInformation("PageType'lar seed işlemi başlatılıyor.");

                    // Bagimliliklar: Once template ve dillerin seed edilmis olmasi gerekir.
                    var templates = await _templateRepository.GetAll()
                        .Where(t => !t.IsDeleted)
                        .ToListAsync(cancellationToken);

                    var diller = await _dilRepository.GetAll()
                        .ToListAsync(cancellationToken);

                    if (!templates.Any() || !diller.Any())
                    {
                        _logger.LogWarning(
                            "PageType seed atlandi. Template sayisi: {TemplateCount}, Dil sayisi: {DilCount}. " +
                            "Lutfen once TemplateSeedService ve DilSeedService'in calismasini saglayin.",
                            templates.Count, diller.Count);
                        await _unitOfWork.RollbackAsync(cancellationToken);
                        return;
                    }

                    var definitions = PageTypeSeedData.GetPageTypeSeedDefinitions();
                    await ProcessPageTypesAsync(templates, diller, definitions, cancellationToken);

                    await _unitOfWork.CommitAsync(cancellationToken);

                    _logger.LogInformation(
                        "PageType'lar seed işlemi tamamlandı. Template: {TemplateCount}, Dil: {DilCount}, Toplam sayfa: {Count}",
                        templates.Count, diller.Count, definitions.Count * templates.Count * diller.Count);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackAsync(cancellationToken);
                    _logger.LogError(ex, "Senkronizasyon hatası");
                    throw;
                }
            });
        }

        private async Task ProcessPageTypesAsync(
            List<Template> templates,
            List<Dil> diller,
            List<PageTypeSeedData.PageTypeSeedDefinition> definitions,
            CancellationToken cancellationToken)
        {
            foreach (var template in templates)
            {
                foreach (var dil in diller)
                {
                    foreach (var definition in definitions)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        var slug = string.Equals(dil.Kod, "TR", StringComparison.OrdinalIgnoreCase)
                            ? definition.SlugTR
                            : definition.SlugEN;

                        await _pageTypeRepository.AddAsync(new PageType {
                            PageTypeKind = definition.Kind,
                            ViewName = definition.ViewName,
                            Slug = slug,
                            TemplateId = template.Id,
                            DilId = dil.Id,
                            // Template1/Index.cshtml ana sayfa gorunumu oldugu icin Home tipi ana sayfa isaretlenir.
                            IsHomePage = definition.Kind == PageTypeKind.Home,
                        });
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
