namespace Microservice.Web.ViewModels.PageSection
{
    /// <summary>
    /// Ana sayfa dinamik bolumu (section) ve icindeki container (block) agaci.
    /// Site API'deki PageSectionDto karsiligi.
    /// </summary>
    public class GetPageSectionVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public string Baslik { get; set; } = default!;
        public string? BackgroundColor { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public int Sira { get; set; }
        public bool Yayinda { get; set; }

        public List<GetPageBlockVm> Blocks { get; set; } = [];
    }
}
