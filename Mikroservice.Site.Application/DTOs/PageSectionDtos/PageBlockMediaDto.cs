namespace Mikroservice.Site.Application.DTOs.PageSectionDtos
{
    public class PageBlockMediaDto
    {
        public int Id { get; set; }
        public int PageBlockId { get; set; }
        public string ResimUrl { get; set; } = default!;
        public string? VideoUrl { get; set; }
        public int Sira { get; set; }
    }

    /// <summary>
    /// Block create/update isteklerinde gönderilen carousel slayt girdisi.
    /// </summary>
    public class PageBlockMediaInputDto
    {
        public string ResimUrl { get; set; } = default!;
        public string? VideoUrl { get; set; }
        public int Sira { get; set; }
    }
}
