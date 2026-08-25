using Microservice.Web.ViewModels.Menu;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Content
{
    public class ContentPageViewModel
    {
        public SiteDetailGetVm Site { get; set; } = null!;
        public List<MenuGetVm> Menus { get; set; } = new();
        public MenuGetVm? CurrentMenu { get; set; }
        public ContentDetailVm Content { get; set; } = null!;
        public string ContentType { get; set; } = string.Empty;
    }
}
