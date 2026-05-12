namespace Microservice.Admin.ViewModels.YoneticiSite
{
    public class YoneticiSiteDetailVm
    {
        public int Id { get; set; }
        public int PersonelId { get; set; } = default!;
        public int SiteId { get; set; }
        public int YoneticiTipiId { get; set; }
    }
}