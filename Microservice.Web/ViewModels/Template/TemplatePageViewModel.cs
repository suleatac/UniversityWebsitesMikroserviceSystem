using Microservice.Web.ViewModels.BandLogo;
using Microservice.Web.ViewModels.Banner;
using Microservice.Web.ViewModels.Bilgi;
using Microservice.Web.ViewModels.Duyuru;
using Microservice.Web.ViewModels.Etkinlik;
using Microservice.Web.ViewModels.GaleriResim;
using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.Menu;
using Microservice.Web.ViewModels.Popup;
using Microservice.Web.ViewModels.ShortcutButton;
using Microservice.Web.ViewModels.Site;
using Microservice.Web.ViewModels.Video;

namespace Microservice.Web.ViewModels.Template
{
    public class TemplatePageViewModel
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        // Link'i bos olan icerikler /{LanguageCode}/{PageTypeSlug}/{SeoUrl} adresine yonlendirilir.
        public string LanguageCode { get; set; } = "tr";

        public List<GetMenuVm> Menus { get; set; } =
            new();

        public GetMenuVm? CurrentMenu { get; set; }

        // Home sayfasına özel en güncel içerikler
        public List<GetBannerVm> Banners { get; set; } = new();
        public List<GetHaberVm> Haberler { get; set; } = new();
        public List<GetDuyuruVm> Duyurular { get; set; } = new();
        public List<GetBilgiVm> Bilgiler { get; set; } = new();
        public List<GetEtkinlikVm> Etkinlikler { get; set; } = new();
        public List<GetShortcutButtonVm> ShortcutButtons { get; set; } = new();
        public string HaberListUrl { get; set; } = "/";
        public string DuyuruListUrl { get; set; } = "/";

        // Template2 ana sayfasi icin ek icerikler
        public GetPopupVm? Popup { get; set; }
        public List<GetVideoVm> Videolar { get; set; } = new();
        public string VideoListUrl { get; set; } = "/";
        public List<GetGaleriResimVm> GaleriResimler { get; set; } = new();
        public string GaleriListUrl { get; set; } = "/";
        public List<GetBandLogoVm> BandLogolar { get; set; } = new();

        // Ana sayfa sayaclari (Template2)
        public int OgrenciSayisi { get; set; }
        public int MezunOgrenciSayisi { get; set; }
        public int IdariPersonelSayisi { get; set; }
        public int AkademikPersonelSayisi { get; set; }
    }
}