using Microservice.Admin.ViewModels.Dil;
using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.ShortcutButton
{
    public class ShortcutButtonCreateIndexVm
    {
        public ShortcutButtonVm CreateShortcutButton { get; set; } = new();
        public List<GetDilVm> Diller { get; set; } = new();
        public List<GetHedefVm> Hedefler { get; set; } = new();
    }
}
