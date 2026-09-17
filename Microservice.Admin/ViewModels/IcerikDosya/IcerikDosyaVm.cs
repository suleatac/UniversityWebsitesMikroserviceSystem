namespace Microservice.Admin.ViewModels.IcerikDosya
{
    /// <summary>
    /// Mikroservice.Site.Application DTOs.IcerikDosyaDtos.IcerikDosyaInputDto karsiligi.
    /// Icerik (Haber/Duyuru) formuna eklenen dosya satirlari icin kullanilir.
    /// Id = 0 -> yeni dosya, Id > 0 -> mevcut dosya guncelleme, listeden cikarilan -> silme.
    /// </summary>
    public class IcerikDosyaVm
    {
        public int Id { get; set; }

        public string? Baslik { get; set; }

        public string DosyaUrl { get; set; } = default!;

        public string DosyaAdi { get; set; } = default!;

        public long DosyaBoyut { get; set; }

        public string? DosyaTuru { get; set; }

        public int Sira { get; set; }
    }
}
