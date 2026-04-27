namespace Microservice.Admin.ViewModels.Site
{
    public class SiteDetailGetVm
    {
        public int Id { get; set; }
        public string SiteAdi { get; set; } = default!;
        public string SiteAdiEng { get; set; } = default!;
        public string SiteUrl { get; set; } = default!;
        public string SiteAlanAdi { get; set; } = default!;
        public string SiteEPosta { get; set; } = default!;
        public int TemplateId { get; set; }
        public int BirimId { get; set; }
        public string SiteEPostaHost { get; set; } = default!;
        public int SiteEPostaPort { get; set; }
        public string SiteEPostaSifre { get; init; } = default!;
        public int SertifikaParmakIziId { get; init; }
    }
}
