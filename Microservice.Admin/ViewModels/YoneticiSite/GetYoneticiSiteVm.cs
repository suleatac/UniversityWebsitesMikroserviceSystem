namespace Microservice.Admin.ViewModels.YoneticiSite
{
    public class GetYoneticiSiteVm
    {
        public int Id { get; set; }
        public string KeycloakUserId { get; set; } = default!;
        public int SiteId { get; set; }
        public int YoneticiTipiId { get; set; }
        public string YoneticiTipiAdi { get; set; } = default!;
    }
}