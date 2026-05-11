using Microservice.Admin.ViewModels.SikcaSorulanSoruKategori;

namespace Microservice.Admin.ViewModels.SikcaSorulanSoru
{
    public class SikcaSorulanSoruCreateIndexVm
    {
        public CreateSikcaSorulanSoruVm CreateSikcaSorulanSoru { get; set; } = new CreateSikcaSorulanSoruVm();
        public List<GetSikcaSorulanSoruKategoriVm> Kategoriler { get; set; } = new List<GetSikcaSorulanSoruKategoriVm>();
    }
}