namespace Mikroservice.Site.Domain.Entities
{
    public class SiteOzellikleri
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string SiteAdress { get; set; } = default!;
        public string SiteAdressEng { get; set; } = default!;
        public string SiteBaslangicHakkimizda { get; set; } = default!;
        public string SiteBaslangicHakkimizdaEng { get; set; } = default!;
        public string SiteTelNo { get; set; } = default!;
        public string SiteFaxNo { get; set; } = default!;
        public string SiteFacebookAdress { get; set; } = default!;
        public string SiteTwitterAdress { get; set; } = default!;
        public string SiteInstagramAdress { get; set; } = default!;
        public string SiteYoutubeAdress { get; set; } = default!;
        public string SiteHaritaAdress { get; set; } = default!;
        public string SiteBaslangicVideoLink { get; set; } = default!;
        public string SiteBaslangicVideoResimAdress { get; set; } = default!;
        public string SiteWatsappAdress { get; set; } = default!;
        public string SiteLinkedinAdress { get; set; } = default!;
        public string SiteHakkindaLink { get; set; } = default!;
        public string SiteVideoType { get; set; } = default!;
        public string SiteHakkindaResim { get; set; } = default!;
        public string SiteFooterLogo { get; set; } = default!;
        public string SiteTopbarLogo { get; set; } = default!;
        public Site Site { get; set; } = default!;

    }
}
