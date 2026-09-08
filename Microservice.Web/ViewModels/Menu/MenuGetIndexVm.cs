using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Menu
{
    public class MenuGetIndexVm
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        public List<MenuGetVm> Menus { get; set; } = new();
    }
}
