using Microservice.Web.ViewModels.Duyuru;
using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.Pages;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.PageRoute
{
    public class RouteResolveResult
    {
        public SiteDetailGetVm Site { get; set; } = null!;
        public PagesDetailVm Page { get; set; } = null!;

        public List<GetHaberVm>? NewsList { get; set; }

        public HaberDetailVm? NewsDetail { get; set; }

        public DuyuruDetailVm? AnnouncementDetail { get; set; }

        public string? DetailSlug { get; set; }
        public int LanguageId { get; set; }

        public string LanguageCode { get; set; } = null!;
    }
}
