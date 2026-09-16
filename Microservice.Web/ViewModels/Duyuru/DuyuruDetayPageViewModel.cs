using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Duyuru
{
    /// <summary>
    /// Duyuru detay sayfasi modeli: detay + iliskili veriler (son eklenen duyurular).
    /// </summary>
    public class DuyuruDetayPageViewModel
    {
        public DuyuruDetailVm Duyuru { get; set; } = null!;

        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Duyuru listesinin adresi: /{dil}/{duyurular-slug} (arama formu ve "tumu" linki icin)
        public string DuyuruListUrl { get; set; } = "/";

        // Sidebar "Son Eklenenler" widget'i
        public List<GetDuyuruVm> LatestDuyurular { get; set; } = new();
    }
}
