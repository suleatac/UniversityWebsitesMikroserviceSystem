using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.SitePersonel
{
    /// <summary>
    /// Personel detay sayfasi modeli: detay + liste adresi + sidebar icin personel listesi.
    /// </summary>
    public class PersonelDetayPageViewModel
    {
        public PersonelDetailVm Personel { get; set; } = null!;

        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Personel listesinin adresi: /{dil}/{personeller-slug}
        public string PersonelListUrl { get; set; } = "/";

        // Sidebar "Son Eklenenler" widget'i
        public List<GetPersonelVm> LatestPersoneller { get; set; } = new();
    }
}
