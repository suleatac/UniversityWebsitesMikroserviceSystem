namespace Microservice.Admin.ViewModels.SitePersonel
{
    public class GetSitePersonelVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int PersonelId { get; set; }
        public int UnvanId { get; set; }
        public int PersonelTipId { get; set; }
        public string ResimUrl { get; set; } = default!;
        public string Hakkinda { get; set; } = default!;
        public string? UnvanAd { get; set; }
        public string? PersonelTipAd { get; set; }
    }
}