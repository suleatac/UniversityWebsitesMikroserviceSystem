using Microservice.Web.Settings;

namespace Microservice.Web.ViewModels.Pages
{
    public class PagesDetailVm
    {
        public int Id { get; set; }

        public PageTypeKindEnum PageTypeKind { get; set; }

        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public int TemplateId { get; set; }

        public string ViewName { get; set; } = null!;

        public bool IsHomePage { get; set; }
    }
}
