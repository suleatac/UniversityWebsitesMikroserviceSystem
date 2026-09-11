using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Menu
{
    public class MenuGetIndexVm
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        public List<MenuGetVm> Menus { get; set; } = new();

        // Link'i bos olan menuler /{LanguageCode}/{PageTypeSlug} adresine yonlendirilir.
        public string LanguageCode { get; set; } = "tr";
    }
}
