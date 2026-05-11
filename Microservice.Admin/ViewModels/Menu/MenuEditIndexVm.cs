using Microservice.Admin.ViewModels.Dil;
using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Menu
{
    public class MenuEditIndexVm
    {
        public MenuVm Menu { get; set; } = new();
        public List<GetMenuVm> Menuler { get; set; } = new();
        public List<GetDilVm> Diller { get; set; } = new();
        public List<GetHedefVm> Hedefler { get; set; } = new();
    }
}