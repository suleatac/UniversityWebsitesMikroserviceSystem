namespace Microservice.Admin.ViewModels.Template
{
    public class UpdateTemplateVm
    {
        public int Id { get; set; }
        public string TemplateAdi { get; set; } = default!;
        public string TemplateTuru { get; set; } = default!;
    }
}