namespace Microservice.Web.ViewModels.Menu
{
    public class MenuGetVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int HedefId { get; set; }

        // API (MenuDto) 'Baslik' alanini doner; 'Ad' eski view'lar icin korundu.
        public string Baslik { get; set; } = default!;
        public string Ad => Baslik;

        public string? Link { get; set; }
        public string? IconUrl { get; set; }
        public string? Icerik { get; set; }
        public int Sira { get; set; }
        public bool MegaMenu { get; set; }

        // MenuLocation: 1=Header, 2=Footer, 3=Sidebar
        public int Location { get; set; } = 1;
        public bool IsVisible { get; set; } = true;

        public int? ParentId { get; set; }
        public List<MenuGetVm> Children { get; set; } = new();
    }
}
