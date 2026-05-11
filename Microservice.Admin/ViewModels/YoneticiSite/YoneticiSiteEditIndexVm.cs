using Microservice.Admin.ViewModels.YoneticiTipi;

namespace Microservice.Admin.ViewModels.YoneticiSite
{
    public class YoneticiSiteEditIndexVm
    {
        public YoneticiSiteDetailVm YoneticiSite { get; set; } = new YoneticiSiteDetailVm();
        public List<GetYoneticiTipiVm> YoneticiTipleri { get; set; } = new List<GetYoneticiTipiVm>();
    }
}