namespace Microservice.Admin.ViewModels.Birim
{
    public class GetBirimDetailVm
    {
        public int Id { get; set; }
        public string Ad { get; set; } = default!;
        public int Sira { get; set; }
        public int? ParentId { get; set; }
    }
}
