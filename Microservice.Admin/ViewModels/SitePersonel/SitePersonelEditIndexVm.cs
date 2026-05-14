using Microservice.Admin.ViewModels.PersonelTip;
using Microservice.Admin.ViewModels.TumPersonel;
using Microservice.Admin.ViewModels.Unvan;

namespace Microservice.Admin.ViewModels.SitePersonel
{
    public class SitePersonelEditIndexVm
    {
        public SitePersonelDetailVm EditSitePersonel { get; set; } = new SitePersonelDetailVm();
        public List<GetUnvanVm> Unvanlar { get; set; } = new List<GetUnvanVm>();
        public List<GetPersonelTipVm> PersonelTipler { get; set; } = new List<GetPersonelTipVm>();
        public List<GetPersonelVm> TumPersoneller { get; set; } = new();
    }
}
