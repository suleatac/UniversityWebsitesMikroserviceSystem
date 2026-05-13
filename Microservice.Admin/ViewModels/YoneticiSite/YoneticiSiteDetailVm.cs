namespace Microservice.Admin.ViewModels.YoneticiSite
{
    public class YoneticiSiteDetailVm
    {
        public int Id { get; set; }
        public string? KeycloakUserId { get; set; }
        public int SiteId { get; set; }
        public string? SiteAdi { get; set; }
        public string? YoneticiTipiAdi { get; set; }
        public string? UserName { get; set; }
    }
}