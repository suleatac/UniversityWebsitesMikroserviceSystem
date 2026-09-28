namespace Mikroservice.Site.Application.DTOs.PageSectionDtos
{
    /// <summary>
    /// Section icindeki container. Children nested satirlari, Medias carousel slaytlarini tasir.
    /// </summary>
    public class PageBlockDto
    {
        public int Id { get; set; }
        public int PageSectionId { get; set; }
        public int? ParentId { get; set; }
        public string ContentType { get; set; } = default!;
        public string? Content { get; set; }
        public string? VideoUrl { get; set; }
        public string? VideoType { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public string? BackgroundColor { get; set; }
        public int ColumnSize { get; set; }
        public int RowNumber { get; set; }
        public string? Animation { get; set; }

        public List<PageBlockMediaDto> Medias { get; set; } = [];
        public List<PageBlockDto> Children { get; set; } = [];
    }
}
