using Microservice.Admin.ViewModels.Dil;
using Microservice.Admin.ViewModels.Hedef;
using Microservice.Admin.ViewModels.Site;

namespace Microservice.Admin.ViewModels.Haber
{
    public class HaberCreateIndexVm
    {
        public CreateHaberVm CreateHaber { get; set; } = new CreateHaberVm();
        public List<SiteGetVm> Siteler { get; set; } = new List<SiteGetVm>();
        public List<GetDilVm> Diller { get; set; } = new List<GetDilVm>();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}
