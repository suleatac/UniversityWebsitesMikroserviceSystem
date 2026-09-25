namespace Microservice.Web.ViewModels.SikcaSorulanSoru
{
    /// <summary>
    /// Site API /api/v1/sss ucundan gelen agac yapisi:
    /// ParentId == null olan kokler kategori, Children ise o kategorinin sorulari.
    /// </summary>
    public class SikcaSorulanSoruVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int? ParentId { get; set; }
        public string Soru { get; set; } = default!;
        public string Cevap { get; set; } = default!;
        public int Sira { get; set; }
        public string? SeoUrl { get; set; }
        public List<SikcaSorulanSoruVm> Children { get; set; } = new();
    }
}
