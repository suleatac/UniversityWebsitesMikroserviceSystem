using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Video
{
    public class VideoEditIndex
    {
        public VideoDetailVm VideoDetail { get; set; } = new VideoDetailVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}
