using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Menu
{
    /// <summary>
    /// Menu (icerik sayfasi) detay modeli.
    /// Menu'nun kendi liste sayfasi olmadigi icin arama widget'i genel arama sayfasina yonlendirir.
    /// </summary>
    public class MenuDetayPageViewModel
    {
        public MenuDetailVm Menu { get; set; } = null!;

        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Genel arama sayfasi adresi: /{dil}/{arama-slug}
        public string SearchUrl { get; set; } = "/";
    }
}
