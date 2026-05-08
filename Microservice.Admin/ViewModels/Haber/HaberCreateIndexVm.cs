using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Haber
{
    public class HaberCreateIndexVm
    {
        public CreateHaberVm CreateHaber { get; set; } = new CreateHaberVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();

    }
}
