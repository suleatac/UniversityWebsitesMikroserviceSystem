using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Domain.Entities
{
    public class Menu : Icerik
    {
        public int? ParentId { get; set; } // 🔥 nullable
        public bool MegaMenu { get; set; }

        // Menunun render edilecegi bolge (ust navbar / footer hizli erisim / sidebar)
        public MenuLocation Location { get; set; } = MenuLocation.Header;

        // Yonetici panelinden gecici olarak gizlemek icin
        public bool IsVisible { get; set; } = true;

        public Menu? Parent { get; set; }
        public ICollection<Menu> Children { get; set; } = new List<Menu>();
    }
}
