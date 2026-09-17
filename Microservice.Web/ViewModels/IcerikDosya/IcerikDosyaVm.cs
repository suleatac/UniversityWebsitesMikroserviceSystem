namespace Microservice.Web.ViewModels.IcerikDosya
{
    /// <summary>
    /// Icerik (Duyuru/Haber vb.) kaydina bagli yuklu dosya.
    /// Site API'deki IcerikDosyaDto karsiligidir.
    /// </summary>
    public class IcerikDosyaVm
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
