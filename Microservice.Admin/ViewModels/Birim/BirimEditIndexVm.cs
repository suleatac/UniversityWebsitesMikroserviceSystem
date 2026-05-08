namespace Microservice.Admin.ViewModels.Birim
{
    public class BirimEditIndexVm
    {
        public UpdateBirimVm Birim { get; set; } = new();
        public List<GetBirimVm> Birimler { get; set; } = new();
    }
}