namespace Mikroservice.Site.Application.DTOs.YoneticiSiteDtos
{
    public class YoneticiSiteDetailDto
    {
        public int Id { get; set; }
        public string? KeycloakUserId { get; set; }
        public int SiteId { get; set; }
        public int YoneticiTipiId { get; set; }
        public string YoneticiTipiAdi { get; set; } = default!;
        public string? SiteAdi { get; set; }
    }
}