using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.GaleriResim
{
    /// <summary>
    /// Galeri resmi detay sayfasi modeli: detay + onceki/sonraki navigasyonu + iliskili resimler.
    /// </summary>
    public class GaleriResimDetayPageViewModel
    {
        public GaleriResimDetailVm GaleriResim { get; set; } = null!;

        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Liste sayfasi adresi: /{dil}/{galeri-resimler-slug}
        public string GaleriResimListUrl { get; set; } = "/";

        // Baslik-resim navigasyonu (Sira -> YayimTarihi siralamasina gore)
        public GetGaleriResimVm? OncekiResim { get; set; }
        public GetGaleriResimVm? SonrakiResim { get; set; }

        // Ayni kategoriden (yetersizse digerlerinden) iliskili resimler
        public List<GetGaleriResimVm> IliskiliResimler { get; set; } = new();
    }
}
