using Microservice.Web.ViewModels.Content;
using Microservice.Web.ViewModels.Duyuru;
using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.Menu;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Template
{
    public class TemplatePageViewModel
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        public List<MenuGetVm> Menus { get; set; } =
            new();

        public MenuGetVm? CurrentMenu { get; set; }

        // Home sayfasına özel en güncel içerikler
        public List<ContentDetailVm> Banners { get; set; } = new();
        public List<GetHaberVm> Haberler { get; set; } = new();
        public List<GetDuyuruVm> Duyurular { get; set; } = new();
    }
}