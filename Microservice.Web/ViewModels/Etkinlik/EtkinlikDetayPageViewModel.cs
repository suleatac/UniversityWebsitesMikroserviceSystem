using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Etkinlik
{
    /// <summary>
    /// Etkinlik detay sayfasi modeli: detay + iliskili veriler (son eklenen etkinlikler).
    /// </summary>
    public class EtkinlikDetayPageViewModel
    {
        public EtkinlikDetailVm Etkinlik { get; set; } = null!;

        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Etkinlik listesinin adresi: /{dil}/{etkinlik-slug}
        public string EtkinlikListUrl { get; set; } = "/";

        // Sidebar "Son Eklenenler" widget'i
        public List<GetEtkinlikVm> LatestEtkinlikler { get; set; } = new();
    }
}
