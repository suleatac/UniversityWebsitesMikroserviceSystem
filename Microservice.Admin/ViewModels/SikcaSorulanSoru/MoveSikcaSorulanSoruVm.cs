namespace Microservice.Admin.ViewModels.SikcaSorulanSoru
{
    public class MoveSikcaSorulanSoruVm
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int Sira { get; set; } = 0;
        public List<int>? SiblingIds { get; set; }
    }
}
