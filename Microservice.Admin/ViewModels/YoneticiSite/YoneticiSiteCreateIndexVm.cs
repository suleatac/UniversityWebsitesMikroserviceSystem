using Microservice.Admin.ViewModels.YoneticiTipi;

namespace Microservice.Admin.ViewModels.YoneticiSite
{
    public class YoneticiSiteCreateIndexVm
    {
        public CreateYoneticiSiteVm CreateYoneticiSite { get; set; } = new CreateYoneticiSiteVm();
        public List<GetYoneticiTipiVm> YoneticiTipleri { get; set; } = new List<GetYoneticiTipiVm>();
    }
}