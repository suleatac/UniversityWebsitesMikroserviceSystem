using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.GaleriResim
{
    public class GaleriResimEditIndexVm
    {
        public GaleriResimDetailVm GaleriResimDetail { get; set; } = new GaleriResimDetailVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}
