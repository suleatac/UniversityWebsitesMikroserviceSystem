namespace Mikroservice.Site.Application.DTOs.PageTypeDtos
{
    using Mikroservice.Site.Domain.Enums;

    public class PageTypeDto
    {
        public int Id { get; set; }
        public PageType PageTypeId { get; set; }
        public int SiteId { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public int TemplateId { get; set; }
        public int DilId { get; set; }
        public string? ViewName { get; set; }
        public bool IsHomePage { get; set; }
        public bool IsActive { get; set; }
    }
}