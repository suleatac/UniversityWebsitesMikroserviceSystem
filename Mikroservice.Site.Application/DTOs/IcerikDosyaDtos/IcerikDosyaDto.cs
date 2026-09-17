namespace Mikroservice.Site.Application.DTOs.IcerikDosyaDtos
{
    /// <summary>
    /// Icerige (Haber/Duyuru) bagli ek dosya. Detay cevaplarinda dondurulur.
    /// </summary>
    public class IcerikDosyaDto
    {
        public int Id { get; set; }
        public int IcerikId { get; set; }
        public string? Baslik { get; set; }
        public string DosyaUrl { get; set; } = default!;
        public string DosyaAdi { get; set; } = default!;
        public long DosyaBoyut { get; set; }
        public string? DosyaTuru { get; set; }
        public int Sira { get; set; }
        public DateTime YuklemeTarihi { get; set; }
    }
}
