using Microservice.Web.ViewModels.Menu;
using Microservice.Web.ViewModels.Seo;
using Microservice.Web.ViewModels.SeoMetadata;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Template
{
    public class TemplatePageViewModel
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        public List<MenuGetVm> Menus { get; set; } =
            new();

        public MenuGetVm? CurrentMenu { get; set; }

        public string? SeoRoute { get; set; }

        public SeoMetadataVm? Seo { get; set; }
    }
}