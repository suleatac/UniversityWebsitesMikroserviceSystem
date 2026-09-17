using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.GaleriResim
{
    public class GaleriResimCreateIndexVm
    {
        public CreateGaleriResimVm CreateGaleriResim { get; set; } = new CreateGaleriResimVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}
