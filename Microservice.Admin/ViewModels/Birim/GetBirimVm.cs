namespace Microservice.Admin.ViewModels.Birim
{
    public class GetBirimVm
    {
        public int Id { get; set; }
        public string Ad { get; set; } = default!;
        public int Sira { get; set; }
        public int? ParentId { get; set; }

        public List<GetBirimVm> Children { get; set; } = new();
    }
}
