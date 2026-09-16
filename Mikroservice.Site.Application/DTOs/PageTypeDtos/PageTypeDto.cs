namespace Mikroservice.Site.Application.DTOs.PageTypeDtos
{
    using Mikroservice.Site.Domain.Enums;

    public class PageTypeDto
    {
        public int Id { get; set; }
        public PageTypeKind PageTypeKind { get; set; }
        public string Slug { get; set; } = default!;
        public int TemplateId { get; set; }
        public int DilId { get; set; }
        public string ViewName { get; set; } = default!;
        public bool IsHomePage { get; set; }
    }
}