using Microservice.Admin.ViewModels.Unvan;
using Microservice.Admin.ViewModels.PersonelTip;

namespace Microservice.Admin.ViewModels.SitePersonel
{
    public class SitePersonelCreateIndexVm
    {
        public CreateSitePersonelVm CreateSitePersonel { get; set; } = new CreateSitePersonelVm();
        public List<GetUnvanVm> Unvanlar { get; set; } = new List<GetUnvanVm>();
        public List<GetPersonelTipVm> PersonelTipler { get; set; } = new List<GetPersonelTipVm>();
    }
}