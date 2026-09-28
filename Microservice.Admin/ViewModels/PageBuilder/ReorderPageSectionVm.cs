namespace Microservice.Admin.ViewModels.PageBuilder
{
    public class ReorderPageSectionItemVm
    {
        public int Id { get; set; }
        public int Sira { get; set; }
    }

    public class ReorderPageSectionsCommandListVm
    {
        public List<ReorderPageSectionItemVm> Items { get; set; } = new();
    }
}
