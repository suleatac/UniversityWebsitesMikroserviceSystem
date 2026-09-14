using Microservice.Web.ViewModels.Bilgi;
using Microservice.Web.ViewModels.Duyuru;
using Microservice.Web.ViewModels.Etkinlik;
using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.Menu;
using Microservice.Web.ViewModels.Pages;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.PageRoute
{
    public class RouteResolveResult
    {
        public SiteDetailGetVm Site { get; set; } = null!;
        public PagesDetailVm Page { get; set; } = null!;

        public List<GetHaberVm>? HaberListesi { get; set; }
        public List<GetDuyuruVm>? DuyuruListesi { get; set; }
        public HaberDetailVm? HaberDetay { get; set; }
        public EtkinlikDetailVm? EtkinlikDetay { get; set; }
        public MenuDetailVm? MenuDetay { get; set; }
        public DuyuruDetailVm? DuyuruDetay { get; set; }
        public BilgiDetailVm? BilgiDetay { get; set; }
        public string? DetailSlug { get; set; }
        public int LanguageId { get; set; }

        public string LanguageCode { get; set; } = null!;
    }
}
