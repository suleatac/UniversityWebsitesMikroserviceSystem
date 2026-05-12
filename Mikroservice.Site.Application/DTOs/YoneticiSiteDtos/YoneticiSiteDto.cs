namespace Mikroservice.Site.Application.DTOs.YoneticiSiteDtos
{
    public class YoneticiSiteDto
    {
        public int Id { get; set; }
        public int PersonelId { get; set; } = default!;
        public int SiteId { get; set; }
        public int YoneticiTipiId { get; set; }
    }
}