namespace Mikroservice.Site.Domain.Entities
{
    public class Unvan
    {
        public int Id { get; set; }
        public int TipId { get; set; }
        public string Ad { get; set; }=default!;
        public string KisaAd { get; set; }=default!;
        public int Sira { get; set; }
        public int? ParentId { get; set; }   // FK (nullable)
        public bool IsDeleted { get; set; } = false;
        public Unvan? Parent { get; set; }   // navigation (parent)
        public ICollection<Unvan> Children { get; set; } = new List<Unvan>(); // navigation (child)
        public PersonelTip PersonelTip { get; set; }=default!;
        public ICollection<SitePersonel> SitePersonels { get; set; } = new List<SitePersonel>();
    }
}
