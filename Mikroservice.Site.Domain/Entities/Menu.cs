namespace Mikroservice.Site.Domain.Entities
{
    public class Menu : Icerik
    {
        public int? ParentId { get; set; } // 🔥 nullable
        public bool MegaMenu { get; set; }
        public Menu? Parent { get; set; }
        public ICollection<Menu> Children { get; set; } = new List<Menu>();
    }
}
