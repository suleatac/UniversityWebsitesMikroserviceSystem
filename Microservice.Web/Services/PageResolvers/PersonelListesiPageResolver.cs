using Microservice.Web.Clients.SitePersonelClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageResolvers
{
    public class PersonelListesiPageResolver : IPageResolver
    {
        private readonly ISitePersonelService _personelService;
        private readonly ILogger<PersonelListesiPageResolver> _logger;

        public PersonelListesiPageResolver(
            ISitePersonelService personelService,
            ILogger<PersonelListesiPageResolver> logger)
        {
            _personelService = personelService;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.PersonelListesi;
        }

        public async Task ResolveAsync(RouteResolveResult result)
        {
            var response = await _personelService.GetPersonelListAsync(result.Site.Id);

            if (!response.IsSuccess || response.Data is null)
            {
                _logger.LogWarning(
                    "Personel listesi bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}",
                    result.Site.Id,
                    result.LanguageId);

                return;
            }

            result.PersonelListesi = response.Data;
        }
    }
}
