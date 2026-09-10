namespace Microservice.Admin.ViewModels.Menu
{
    public class GetMenuVm
    {
        public int Id { get; set; }
        public int PageTypeId { get; set; }
        public string Baslik { get; set; } = default!;
        public string? Link { get; set; } = default!;
        public string? IcerikMetni { get; set; }
        public int Sira { get; set; }
        public bool MegaMenu { get; set; }
        public int Location { get; set; } = 1;
        public bool IsVisible { get; set; } = true;
        public int? ParentId { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int HedefId { get; set; }
        public List<GetMenuVm> Children { get; set; } = new();
    }
}