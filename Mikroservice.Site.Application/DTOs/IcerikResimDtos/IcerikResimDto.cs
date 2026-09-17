namespace Mikroservice.Site.Application.DTOs.IcerikResimDtos
{
    /// <summary>
    /// Icerige (Haber/Duyuru) bagli galeri resmi. Detay cevaplarinda dondurulur.
    /// </summary>
    public class IcerikResimDto
    {
        public int Id { get; set; }
        public int IcerikId { get; set; }
        public string? Baslik { get; set; }
        public string ResimUrl { get; set; } = default!;
        public int Sira { get; set; }
        public DateTime YuklemeTarihi { get; set; }
    }
}
