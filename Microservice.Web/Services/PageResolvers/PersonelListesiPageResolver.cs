using Microservice.Web.Clients.SitePersonelClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageResolvers
{
    public class PersonelListesiPageResolver : IPageResolver
    {
        private readonly ISitePersonelClientServices _personelClient;
        private readonly ILogger<PersonelListesiPageResolver> _logger;

        public PersonelListesiPageResolver(
            ISitePersonelClientServices personelClient,
            ILogger<PersonelListesiPageResolver> logger)
        {
            _personelClient = personelClient;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.PersonelListesi;
        }

        public async Task ResolveAsync(RouteResolveResult result)
        {
            var response = await _personelClient.GetPersonelListAsync(result.Site.Id);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Personel listesi bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}",
                    result.Site.Id,
                    result.LanguageId);

                return;
            }

            result.PersonelListesi = response.Content;
        }
    }
}
