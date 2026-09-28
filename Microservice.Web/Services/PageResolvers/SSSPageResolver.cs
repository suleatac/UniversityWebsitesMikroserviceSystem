using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageResolvers
{
    public class SSSPageResolver : IPageResolver
    {
        private readonly ISikcaSorulanSoruService _sikcaSorulanSoruService;
        private readonly ILogger<SSSPageResolver> _logger;

        public SSSPageResolver(
            ISikcaSorulanSoruService sikcaSorulanSoruService,
            ILogger<SSSPageResolver> logger)
        {
            _sikcaSorulanSoruService = sikcaSorulanSoruService;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.SSS;
        }

        public async Task ResolveAsync(RouteResolveResult result)
        {
            var sssResult = await _sikcaSorulanSoruService
        .GetSikcaSorulanSorularAsync(result.Site.Id, result.LanguageId);

            if (!sssResult.IsSuccess || sssResult.Data is null)
            {
                _logger.LogWarning(
                    "SSS listesi bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}",
                    result.Site.Id,
                    result.LanguageId);

                return;
            }

            result.SikcaSorulanSoruListesi = sssResult.Data;
        }
    }
}
