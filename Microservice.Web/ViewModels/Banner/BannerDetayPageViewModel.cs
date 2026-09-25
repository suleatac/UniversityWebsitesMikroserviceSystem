using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Banner
{
    /// <summary>
    /// Banner detay sayfasi modeli: banner icerigi + genel arama adresi + sidebar icin diger bannerlar.
    /// </summary>
    public class BannerDetayPageViewModel
    {
        public BannerDetailVm Banner { get; set; } = null!;

        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Genel arama sayfasi adresi (sidebar arama widget'i icin)
        public string SearchUrl { get; set; } = "/";

        // Sidebar "Diger Bannerlar" widget'i
        public List<GetBannerVm> OtherBanners { get; set; } = new();
    }
}
