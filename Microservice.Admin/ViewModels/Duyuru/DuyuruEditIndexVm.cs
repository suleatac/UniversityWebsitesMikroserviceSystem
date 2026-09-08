using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Duyuru
{
    public class DuyuruEditIndexVm
    {
        public DuyuruDetailVm DuyuruDetail { get; set; } = new DuyuruDetailVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}
