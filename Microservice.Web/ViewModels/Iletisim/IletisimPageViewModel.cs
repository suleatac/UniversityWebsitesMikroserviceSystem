using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Iletisim
{
    /// <summary>
    /// Iletisim sayfasi modeli: form + sitenin iletisim bilgileri (mail, tel, adres, harita)
    /// + sidebar "Son Eklenenler" widget'i icin en yeni haberler.
    /// </summary>
    public class IletisimPageViewModel
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Form alanlari icin model
        public IletisimMesajiVm Form { get; set; } = new();

        // Sidebar arama widget'inin gidecegi genel arama sayfasi adresi
        public string SearchUrl { get; set; } = "/";

        // Sidebar "Son Eklenenler" widget'i
        public List<GetHaberVm> LatestHabers { get; set; } = new();
    }
}
