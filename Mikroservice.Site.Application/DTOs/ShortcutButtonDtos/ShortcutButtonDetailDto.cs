namespace Mikroservice.Site.Application.DTOs.ShortcutButtonDtos
{
    public class ShortcutButtonDetailDto
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int HedefId { get; set; }
        public string Ad { get; set; } = default!;
        public string? Link { get; set; } = default!;
        public string? IconUrl { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsIconImage { get; set; }
        public int Sira { get; set; }

    }
}
