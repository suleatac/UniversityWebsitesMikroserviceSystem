namespace Mikroservice.Site.Domain.Entities
{
    public class Site
    {
        public int Id { get; set; }
        public string SiteAdi { get; set; } = default!;
        public string SiteAdiEng { get; set; } = default!;
        public string SiteUrl { get; set; } = default!;
        public int BirimId { get; set; }
        public string SiteAlanAdi { get; set; } = default!;
        public string SiteEPostaSifre { get; set; } = default!;
        public string SiteEPostaHost { get; set; } = default!;
        public int SiteEPostaPort { get; set; }
        public int SertifikaParmakIziId { get; set; }
        public int TemplateId { get; set; }
        public bool IsDeleted { get; set; }
        public string SiteEPosta { get; set; } = default!;
        public Template Template { get; set; } = default!;
        public Birim Birim { get; set; } = default!;
        public SertifikaParmakIzi SertifikaParmakIzi { get; set; } = default!;
        public SiteOzellikleri SiteOzellikleri { get; set; } = default!;
        public ICollection<YoneticiSite> YoneticiSites { get; set; } = new List<YoneticiSite>();
        public ICollection<SitePersonel> SitePersonels { get; set; } = new List<SitePersonel>();
    }
}
