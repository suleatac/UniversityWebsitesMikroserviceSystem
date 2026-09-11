using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Application.DTOs.MenuDtos
{
    // Menu DTO'su icinde eager loading ile dondurulen kisa PageType bilgisi
    public class MenuPageTypeDto
    {
        public int Id { get; set; }
        public PageTypeKind PageTypeKind { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public int TemplateId { get; set; }
        public string ViewName { get; set; } = default!;
        public bool IsHomePage { get; set; }
    }
}
