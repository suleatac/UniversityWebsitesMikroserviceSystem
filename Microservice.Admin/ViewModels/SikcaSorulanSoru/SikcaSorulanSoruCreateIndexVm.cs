namespace Microservice.Admin.ViewModels.SikcaSorulanSoru
{
    public class SikcaSorulanSoruCreateIndexVm
    {
        public CreateSikcaSorulanSoruVm CreateSikcaSorulanSoru { get; set; } = new CreateSikcaSorulanSoruVm();
       public List<GetSikcaSorulanSoruVm> SikcaSorulanSorular { get; set; } = new List<GetSikcaSorulanSoruVm>();
    }
}
