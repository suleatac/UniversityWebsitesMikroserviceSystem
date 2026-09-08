using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Bilgi
{
    public class BilgiEditIndexVm
    {
        public BilgiDetailVm BilgiDetail { get; set; } = new BilgiDetailVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}
