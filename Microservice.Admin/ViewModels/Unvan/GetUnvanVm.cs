namespace Microservice.Admin.ViewModels.Unvan
{
    public class GetUnvanVm
    {
        public int Id { get; set; }
        public int TipId { get; set; }
        public string Ad { get; set; } = default!;
        public string KisaAd { get; set; } = default!;
        public int Sira { get; set; }
        public int? ParentId { get; set; }
        public List<GetUnvanVm> Children { get; set; } = new();
    }
}
