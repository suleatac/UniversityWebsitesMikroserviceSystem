using Microservice.Web.ViewModels.Duyuru;
using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.Pages;

namespace Microservice.Web.ViewModels.PageRoute
{
    public class RouteResolveResult
    {
        public PagesDetailVm Page { get; set; } = null!;

        public HaberDetailVm? New { get; set; }

        public DuyuruDetailVm? Announcement { get; set; }

        public string? DetailSlug { get; set; }
        public int LanguageId { get; set; }

        public string LanguageCode { get; set; } = null!;
    }
}
