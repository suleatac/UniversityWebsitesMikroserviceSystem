namespace Microservice.Admin.ViewModels.SikcaSorulanSoru
{
    public class SikcaSorulanSoruEditIndexVm
    {
        public SikcaSorulanSoruDetailVm SikcaSorulanSoru { get; set; } = new();
        public List<GetSikcaSorulanSoruVm> SikcaSorulanSorular { get; set; } = new();
    }
}
