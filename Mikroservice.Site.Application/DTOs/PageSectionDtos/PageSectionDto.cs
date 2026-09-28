namespace Mikroservice.Site.Application.DTOs.PageSectionDtos
{
    /// <summary>
    /// Dinamik sayfa bolumu (section) + icindeki container (block) agaci.
    /// Web tarafi bu DTO ile Template3 section'larini render eder.
    /// </summary>
    public class PageSectionDto
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public string Baslik { get; set; } = default!;
        public string? BackgroundColor { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public int Sira { get; set; }
        public bool Yayinda { get; set; }

        public List<PageBlockDto> Blocks { get; set; } = [];
    }
}
