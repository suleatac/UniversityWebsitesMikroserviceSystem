namespace Mikroservice.Site.Application.DTOs.MenuDtos
{
    public class MenuDetailDto
    {
        public int Id { get; set; }

        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int HedefId { get; set; }
        public int PageTypeId { get; set; }

        public string Baslik { get; set; } = default!;
        public string? Link { get; set; } 
        public string? IcerikMetni { get; set; }

        public int Sira { get; set; }
        public bool MegaMenu { get; set; }

        public int? ParentId { get; set; }

        public string SeoUrl { get; set; } = default!;
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }



    }
}