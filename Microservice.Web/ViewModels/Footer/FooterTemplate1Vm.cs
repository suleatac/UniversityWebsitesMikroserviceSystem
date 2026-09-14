using Microservice.Web.ViewModels.BandLogo;
using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.Menu;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Footer
{
    public class FooterTemplate1Vm
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        // Link'i bos olan menuler /{LanguageCode}/{PageTypeSlug} adresine yonlendirilir.
        public string LanguageCode { get; set; } = "tr";

        public List<GetBandLogoVm> BandLogos { get; set; } = new();

        // HIZLI ERISIM: Location=Footer olan kok menuler (sutunlar), children'lari ile birlikte
        public List<GetMenuVm> FooterColumns { get; set; } = new();

        // Son haberler (RECENT POSTS alani)
        public List<GetHaberVm> LatestHaberler { get; set; } = new();
    }
}
