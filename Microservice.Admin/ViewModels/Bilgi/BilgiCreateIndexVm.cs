using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Bilgi
{
    public class BilgiCreateIndexVm
    {
        public CreateBilgiVm CreateBilgi { get; set; } = new CreateBilgiVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}