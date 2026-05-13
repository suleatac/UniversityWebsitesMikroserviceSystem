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
        public int TemplateId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string SiteEPosta { get; set; } = default!;

        public Template Template { get; set; } = default!;
        public Birim Birim { get; set; } = default!;
        public SiteOzellikleri SiteOzellikleri { get; set; } = default!;
        public ICollection<YoneticiSite> YoneticiSites { get; set; } = new List<YoneticiSite>();
        public ICollection<SitePersonel> SitePersonels { get; set; } = new List<SitePersonel>();
        public ICollection<Menu> Menus { get; set; } = new List<Menu>();
        public ICollection<Haber> Habers { get; set; } = new List<Haber>();
        public ICollection<Duyuru> Duyurus { get; set; } = new List<Duyuru>();
        public ICollection<BandLogo> BandLogos { get; set; } = new List<BandLogo>();
        public ICollection<MediaFile> MediaFiles { get; set; } = new List<MediaFile>();
        public ICollection<SikcaSorulanSoru> SikcaSorulanSorus { get; set; } = new List<SikcaSorulanSoru>();
        public Popup? Popup { get; set; }
    }
}
