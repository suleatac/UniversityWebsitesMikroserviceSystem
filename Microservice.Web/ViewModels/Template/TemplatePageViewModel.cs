using Microservice.Web.ViewModels.Banner;
using Microservice.Web.ViewModels.Bilgi;
using Microservice.Web.ViewModels.Duyuru;
using Microservice.Web.ViewModels.Etkinlik;
using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.Menu;
using Microservice.Web.ViewModels.ShortcutButton;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Template
{
    public class TemplatePageViewModel
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        // Link'i bos olan icerikler /{LanguageCode}/{PageTypeSlug}/{SeoUrl} adresine yonlendirilir.
        public string LanguageCode { get; set; } = "tr";

        public List<MenuGetVm> Menus { get; set; } =
            new();

        public MenuGetVm? CurrentMenu { get; set; }

        // Home sayfasına özel en güncel içerikler
        public List<GetBannerVm> Banners { get; set; } = new();
        public List<GetHaberVm> Haberler { get; set; } = new();
        public List<GetDuyuruVm> Duyurular { get; set; } = new();
        public List<BilgiVm> Bilgiler { get; set; } = new();
        public List<EtkinlikVm> Etkinlikler { get; set; } = new();
        public List<GetShortcutButtonVm> ShortcutButtons { get; set; } = new();
    }
}