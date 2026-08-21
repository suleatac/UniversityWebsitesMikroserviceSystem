namespace Microservice.Web.ViewModels.Menu
{
    public class MenuGetVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int HedefId { get; set; }
        public string Ad { get; set; } = default!;
        public string? Link { get; set; }
        public string? IconUrl { get; set; }
        public string? Icerik { get; set; }
        public int Sira { get; set; }
        public bool MegaMenu { get; set; }
        public int? ParentId { get; set; }
        public List<MenuGetVm> Children { get; set; } = new();
    }
}