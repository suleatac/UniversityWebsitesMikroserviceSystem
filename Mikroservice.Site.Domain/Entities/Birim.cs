namespace Mikroservice.Site.Domain.Entities
{
    public class Birim
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }   // FK (nullable)

        public string Ad { get; set; } = default!;

        public bool IsDeleted { get; set; }

        public int Sira { get; set; }

        public Birim? Parent { get; set; }   // navigation (parent)

        public ICollection<Birim> Children { get; set; } = new List<Birim>(); // navigation (child)
        public ICollection<Site> Sites { get; set; } = new List<Site>();
    }
}
