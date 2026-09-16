using Microservice.Web.ViewModels.Site;
using Microservice.Web.ViewModels.SitePersonel;

namespace Microservice.Web.ViewModels.SitePersonel
{
    /// <summary>
    /// Personel listesi sayfasi modeli.
    /// </summary>
    public class PersonelListPageViewModel
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Sayfa ici arama (?q=...) — client-side filtreleme icin
        public string? Query { get; set; }

        public List<GetPersonelVm> Personeller { get; set; } = new();
    }
}
