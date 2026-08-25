namespace Microservice.Web.ViewModels.Pages
{
    public class PagesDetailVm
    {
        public int Id { get; set; }

        public int SiteId { get; set; }
        public int DilId { get; set; }

        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public int PageTypeId { get; set; }

        public int TemplateId { get; set; }

        public string? ViewName { get; set; }

        public bool IsHomePage { get; set; }
        public bool IsActive { get; set; }
    }
}
