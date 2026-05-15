namespace Microservice.Admin.ViewModels.Menu
{
    public class GetMenuVm
    {
        public int Id { get; set; }
        public string Ad { get; set; } = default!;
        public string? Link { get; set; } = default!;
        public string? IconUrl { get; set; }
        public string? Icerik { get; set; }
        public int Sira { get; set; }
        public bool MegaMenu { get; set; }
        public int? ParentId { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int HedefId { get; set; }
        public List<GetMenuVm> Children { get; set; } = new();
    }
}