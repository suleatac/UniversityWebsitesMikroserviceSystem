namespace Microservice.Admin.ViewModels.Template
{
    public class CreateTemplateVm
    {
        public string TemplateAdi { get; set; } = default!;
        public string TemplateTuru { get; set; } = default!;
        public string? FolderName { get; set; } = default!;
        public string? LayoutPath { get; set; } = default!;
    }
}
