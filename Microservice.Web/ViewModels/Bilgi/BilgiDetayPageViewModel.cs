using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Bilgi
{
    /// <summary>
    /// Bilgi detay sayfasi modeli: detay + iliskili veriler (son eklenen bilgiler).
    /// </summary>
    public class BilgiDetayPageViewModel
    {
        public BilgiDetailVm Bilgi { get; set; } = null!;

        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Bilgi listesinin adresi: /{dil}/{bilgi-slug}
        public string BilgiListUrl { get; set; } = "/";

        // Sidebar "Son Eklenenler" widget'i
        public List<GetBilgiVm> LatestBilgiler { get; set; } = new();
    }
}
