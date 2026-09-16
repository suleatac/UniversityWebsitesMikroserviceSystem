using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Haber
{
    /// <summary>
    /// Haber detay sayfasi modeli: detay + iliskili veriler (son eklenen haberler).
    /// </summary>
    public class HaberDetayPageViewModel
    {
        public HaberDetailVm Haber { get; set; } = null!;

        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Haber listesinin adresi: /{dil}/{haberler-slug} (arama formu ve "tumu" linki icin)
        public string HaberListUrl { get; set; } = "/";

        // Sidebar "Son Eklenenler" widget'i
        public List<GetHaberVm> LatestHabers { get; set; } = new();
    }
}
