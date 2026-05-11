using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Duyuru
{
    public class DuyuruCreateIndexVm
    {
        public CreateDuyuruVm CreateDuyuru { get; set; } = new CreateDuyuruVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}