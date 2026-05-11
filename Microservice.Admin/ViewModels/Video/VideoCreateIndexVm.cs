using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Video
{
    public class VideoCreateIndexVm
    {
        public CreateVideoVm CreateVideo { get; set; } = new CreateVideoVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}