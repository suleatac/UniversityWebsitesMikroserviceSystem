namespace Mikroservice.Site.Application.DTOs.SitePersonelDtos
{
    public class SitePersonelDetailDto
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int PersonelId { get; set; }
        public int UnvanId { get; set; }
        public int PersonelTipId { get; set; }
        public string? ResimUrl { get; set; }
        public string? Hakkinda { get; set; }
        public string? UnvanAd { get; set; }
        public string? PersonelTipAd { get; set; }
    }
}
