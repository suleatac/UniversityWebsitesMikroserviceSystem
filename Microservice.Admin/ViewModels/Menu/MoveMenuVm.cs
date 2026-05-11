namespace Microservice.Admin.ViewModels.Menu
{
    public class MoveMenuVm
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int Sira { get; set; }
        public List<int>? SiblingIds { get; set; }
    }
}