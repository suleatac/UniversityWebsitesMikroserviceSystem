using Microservice.Admin.ViewModels.Site;
using Microservice.Admin.ViewModels.TumPersonel;
using Microservice.Admin.ViewModels.User;

namespace Microservice.Admin.ViewModels.YoneticiSite
{
    public class YoneticiSiteEditIndexVm
    {
        public List<GetPersonelVm> TumPersoneller { get; set; } = new List<GetPersonelVm>();
        public YoneticiSiteDetailVm EditYoneticiSite { get; set; } = new YoneticiSiteDetailVm();
        public List<SiteGetVm> TumSiteler { get; set; } = new List<SiteGetVm>();
        public List<UserListVm> Users { get; set; } = new List<UserListVm>();
    }
}