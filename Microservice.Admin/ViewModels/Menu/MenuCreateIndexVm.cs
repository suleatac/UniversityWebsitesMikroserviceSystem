using Microservice.Admin.ViewModels.Dil;
using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Menu
{
    public class MenuCreateIndexVm
    {
        public MenuVm CreateMenu { get; set; } = new();
        public List<GetMenuVm> Menuler { get; set; } = new();
        public List<GetDilVm> Diller { get; set; } = new();
        public List<GetHedefVm> Hedefler { get; set; } = new();
    }
}