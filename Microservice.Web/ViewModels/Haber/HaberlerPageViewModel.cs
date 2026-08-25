using Microservice.Web.ViewModels.Menu;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Haber
{
    public class HaberlerPageViewModel
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        public List<MenuGetVm> Menus { get; set; } =
            new();

        public MenuGetVm? CurrentMenu { get; set; }

        public List<GetHaberVm> Haberler { get; set; } = null!;
    }
}
