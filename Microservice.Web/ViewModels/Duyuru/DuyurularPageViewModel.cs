using Microservice.Web.ViewModels.Menu;
using Microservice.Web.ViewModels.SeoMetadata;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Duyuru
{
    public class DuyurularPageViewModel
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        public List<MenuGetVm> Menus { get; set; } =
            new();

        public MenuGetVm? CurrentMenu { get; set; }

        public DuyuruDetailVm Duyuru { get; set; } = null!;

        public SeoMetadataVm Seo { get; set; } = null!;
    }
}
