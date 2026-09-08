using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Etkinlik
{
    public class EtkinlikEditIndexVm
    {
        public EtkinlikDetailVm EtkinlikDetail { get; set; } = new EtkinlikDetailVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}
