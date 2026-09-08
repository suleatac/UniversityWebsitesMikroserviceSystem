using Microservice.Admin.ViewModels.Hedef;

namespace Microservice.Admin.ViewModels.Haber
{
    public class HaberEditIndexVm
    {
        public HaberDetailVm HaberDetail { get; set; } = new HaberDetailVm();
        public List<GetHedefVm> Hedefler { get; set; } = new List<GetHedefVm>();
    }
}
