namespace Mikroservice.Site.Application.DTOs.YoneticiSiteDtos
{
    public class YoneticiSiteDto
    {
        public int Id { get; set; }
        public string? KeycloakUserId { get; set; }
        public int SiteId { get; set; }
        public int YoneticiTipiId { get; set; }
    }
}