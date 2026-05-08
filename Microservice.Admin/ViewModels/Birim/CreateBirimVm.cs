namespace Microservice.Admin.ViewModels.Birim
{
    public class CreateBirimVm
    {
        public int? ParentId { get; init; }

        public string Ad { get; init; } = default!;

        public int Sira { get; set; }
    }
}
