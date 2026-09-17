namespace Microservice.Web.ViewModels.IcerikResim
{
    /// <summary>
    /// Icerik (Duyuru/Haber vb.) kaydina bagli galeri resimi.
    /// Site API'deki IcerikResimDto karsiligidir.
    /// </summary>
    public class IcerikResimVm
    {
        public int Id { get; set; }
        public int IcerikId { get; set; }
        public string? Baslik { get; set; }
        public string ResimUrl { get; set; } = default!;
        public int Sira { get; set; }
        public DateTime YuklemeTarihi { get; set; }
    }
}
