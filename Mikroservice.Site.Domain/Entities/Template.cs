namespace Mikroservice.Site.Domain.Entities
{
    public class Template
    {
        public int Id { get; set; }
        public string TemplateAdi { get; set; } = default!;
        public string TemplateTuru { get; set; } = default!;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Site> Sites { get; set; } = new List<Site>();
    }
}
