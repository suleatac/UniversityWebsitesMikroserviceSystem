using Microservice.Admin.ViewModels.Hedef;
using Microservice.Admin.ViewModels.PageType;

namespace Microservice.Admin.ViewModels.Banner
{
    public class BannerCreateIndexVm
    {
        public CreateBannerVm CreateBanner { get; set; } = new CreateBannerVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
        public List<GetPageTypeVm> PageTypes { get; set; } = new List<GetPageTypeVm>();
    }
}