using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Popup
{
    public class PopupCreateIndexVm
    {
        public CreatePopupVm CreatePopup { get; set; } = new CreatePopupVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}