namespace Microservice.Admin.ViewModels.Birim
{
    public class UpdateBirimVm
    {
        public int Id { get; init; }
        public int? ParentId { get; init; }

        public string Ad { get; init; } = default!;

        public int Sira { get; init; }
    }
}
