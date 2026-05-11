using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Banner
{
    public class BannerCreateIndexVm
    {
        public CreateBannerVm CreateBanner { get; set; } = new CreateBannerVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}