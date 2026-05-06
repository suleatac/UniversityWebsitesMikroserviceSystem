using Microservice.Admin.ViewModels.Birim;
using Microservice.Admin.ViewModels.Template;

namespace Microservice.Admin.ViewModels.Site
{
    public class SiteIndexVm
    {
        public CreateSiteVm CreateSite { get; set; } = new CreateSiteVm();
        public List<GetBirimVm> Birimler { get; set; } = new List<GetBirimVm>();
        public List<GetTemplateVm> Templates { get; set; } = new List<GetTemplateVm>();
    }
}
