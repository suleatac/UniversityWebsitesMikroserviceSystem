namespace Mikroservice.Site.Application.DTOs.BandLogoDtos
{
    public class BandLogoDto
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public string Ad { get; set; } = default!;
        public string ImgUrl { get; set; } = default!;
        public string? Link { get; set; }
        public DateTime EklenmeTarihi { get; set; }
    }
}
