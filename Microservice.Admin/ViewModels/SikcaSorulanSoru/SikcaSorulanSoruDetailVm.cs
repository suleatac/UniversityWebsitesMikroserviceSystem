namespace Microservice.Admin.ViewModels.SikcaSorulanSoru
{
    public class SikcaSorulanSoruDetailVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int KategoriId { get; set; }
        public string Soru { get; set; } = default!;
        public string Cevap { get; set; } = default!;
        public int Sira { get; set; }
        public string? SeoUrl { get; set; }
    }
}