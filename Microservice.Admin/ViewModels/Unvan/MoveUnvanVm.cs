namespace Microservice.Admin.ViewModels.Unvan
{
    public class MoveUnvanVm
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int Sira { get; set; }
        public List<int>? SiblingIds { get; set; }
    }
}
