namespace Microservice.Admin.ViewModels.Birim
{
    public class BirimCreateIndexVm
    {
        public CreateBirimVm CreateBirim { get; set; } = new();
        public List<GetBirimVm> Birimler { get; set; } = new();
    }
}