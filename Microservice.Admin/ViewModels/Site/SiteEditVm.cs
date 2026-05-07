using Microservice.Admin.ViewModels.Birim;
using Microservice.Admin.ViewModels.Template;

namespace Microservice.Admin.ViewModels.Site
{
    public class SiteEditVm
    {
        public SiteDetailGetVm Site { get; set; } = new();
        public List<GetBirimVm> Birimler { get; set; } = new();
        public List<GetTemplateVm> Templates { get; set; } = new();
    }
}
