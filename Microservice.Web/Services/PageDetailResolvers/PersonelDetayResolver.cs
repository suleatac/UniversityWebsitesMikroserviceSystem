using Microservice.Web.Clients.DuyuruClients;
using Microservice.Web.Clients.SitePersonelClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageDetailResolvers
{
    public class PersonelDetayResolver : IPageDetailResolver
    {
        private readonly ISitePersonelClientServices _personelClient;
        private readonly ILogger<PersonelDetayResolver> _logger;

        public PersonelDetayResolver(
            ISitePersonelClientServices personelClient,
            ILogger<PersonelDetayResolver> logger)
        {
            _personelClient = personelClient;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.PersonelDetay;
        }

        public async Task<RouteResolveResult?> ResolveAsync(
            RouteResolveResult result,
            string detailSlug)
        {
            var response = await _personelClient.GetPersonelBySeoUrlAsync(result.Site.Id, detailSlug);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Personel bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}, SeoUrl: {SeoUrl}",
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

                return null;
            }

            result.PersonelDetay = response.Content;

            return result;
        }
    }
}
