namespace Mikroservice.Site.Domain.Entities
{
    public class Menu
    {
    public int Id { get; set; }

    public int SiteId { get; set; }
    public int DilId { get; set; }
    public int HedefId { get; set; }
    public int? ParentId { get; set; } // 🔥 nullable

    public string Ad { get; set; } = default!;
    public string Link { get; set; } = default!;
    public string? IconUrl { get; set; }
    public string? Icerik { get; set; } // opsiyonel (istersen kaldır)
    
    public int Sira { get; set; }
    public bool MegaMenu { get; set; }
    public DateTime OlusturulmaTarihi { get; set; }
    public bool IsDeleted { get; set; }

    // 🔥 NAVIGATION
    public Site Site { get; set; } = default!;
    public Dil Dil { get; set; } = default!;
    public Hedef Hedef { get; set; } = default!;
    public Menu? Parent { get; set; }

    public ICollection<Menu> Children { get; set; } = new List<Menu>();
    }
}
