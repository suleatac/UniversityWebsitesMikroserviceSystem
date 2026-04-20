namespace Mikroservice.Site.Domain.Entities
{
    public class Template
    {
        public int Id { get; set; }
        public string TemplateAdi { get; set; } = default!;
        public string TemplateTuru { get; set; } = default!;
        public string FolderName { get; set; } = default!;
        public string LayoutPath { get; set; } = default!;
        public ICollection<Site> Sites { get; set; } = new List<Site>();
    }
}
