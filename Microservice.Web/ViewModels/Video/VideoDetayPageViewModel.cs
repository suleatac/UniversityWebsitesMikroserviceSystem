using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Video
{
    /// <summary>
    /// Video detay sayfasi modeli: detay + liste adresi + sidebar icin en yeni videolar.
    /// </summary>
    public class VideoDetayPageViewModel
    {
        public VideoDetailVm Video { get; set; } = null!;

        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Video listesinin adresi: /{dil}/{videolar-slug}
        public string VideoListUrl { get; set; } = "/";

        // Genel arama sayfasi adresi (sidebar arama widget'i icin)
        public string SearchUrl { get; set; } = "/";

        // Sidebar "Son Eklenenler" widget'i
        public List<GetVideoVm> LatestVideolar { get; set; } = new();
    }
}
