namespace Mikroservice.Site.Application.DTOs.TemplateDtos
{
    public class TemplateDetailDto
    {
        public int Id { get; set; }
        public string TemplateAdi { get; set; } = default!;
        public string TemplateTuru { get; set; } = default!;
    }
}
