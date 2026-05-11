using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Etkinlik
{
    public class EtkinlikCreateIndexVm
    {
        public CreateEtkinlikVm CreateEtkinlik { get; set; } = new CreateEtkinlikVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}