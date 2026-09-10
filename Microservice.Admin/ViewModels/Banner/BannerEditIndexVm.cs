using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Banner
{
    public class BannerEditIndexVm
    {
        public BannerDetailVm BannerDetail { get; set; } = new BannerDetailVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}
