namespace Microservice.Admin.ViewModels.SikcaSorulanSoru
{
    public class CreateSikcaSorulanSoruVm
    {
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int? ParentId { get; set; }

        public string Soru { get; set; } = default!;
        public string Cevap { get; set; } = default!;

        public int Sira { get; set; } = 0;

        public string? SeoUrl { get; set; }
    }
}
