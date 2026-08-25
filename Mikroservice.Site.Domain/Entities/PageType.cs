namespace Mikroservice.Site.Domain.Entities
{
    public class PageType
    {
        public int Id { get; set; }
        public Enums.PageType PageTypeId { get; set; }
        public int SiteId { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public int TemplateId { get; set; }
        public string? ViewName { get; set; }
        public int DilId { get; set; }
        public bool IsHomePage { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        public Dil Dil { get; set; } = default!;
        public Site Site { get; set; } = default!;
        public Template Template { get; set; } = default!;
        public ICollection<Icerik> Icerikler { get; set; } = new List<Icerik>();
        public ICollection<Menu> Menuler { get; set; } = new List<Menu>();
        public ICollection<Popup> Popuplar { get; set; } = new List<Popup>();
    }
}