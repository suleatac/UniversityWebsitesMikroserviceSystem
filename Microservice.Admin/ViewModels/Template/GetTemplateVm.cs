namespace Microservice.Admin.ViewModels.Template
{
    public class GetTemplateVm
    {
        public int Id { get; set; }
        public string TemplateAdi { get; set; } = default!;
        public string TemplateTuru { get; set; } = default!;
        public string? FolderName { get; set; }
        public string? LayoutPath { get; set; }
    }
}
