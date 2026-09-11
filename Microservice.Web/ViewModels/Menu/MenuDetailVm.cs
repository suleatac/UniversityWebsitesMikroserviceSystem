using Microservice.Web.ViewModels.Pages;

namespace Microservice.Web.ViewModels.Menu
{
    public class MenuDetailVm
    {
        public int Id { get; set; }

        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int HedefId { get; set; }
        public int PageTypeId { get; set; }

        public string Baslik { get; set; } = default!;
        public string? Link { get; set; }
        public string? IcerikMetni { get; set; }

        public int Sira { get; set; }
        public bool MegaMenu { get; set; }

        public int Location { get; set; }
        public bool IsVisible { get; set; } = true;

        public int? ParentId { get; set; }

        public string SeoUrl { get; set; } = default!;
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public PagesDetailVm PageType { get; set; } = null!;

    }
}
