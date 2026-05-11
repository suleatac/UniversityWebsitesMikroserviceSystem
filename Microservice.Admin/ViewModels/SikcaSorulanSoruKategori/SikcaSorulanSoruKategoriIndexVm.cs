namespace Microservice.Admin.ViewModels.SikcaSorulanSoruKategori
{
    public class SikcaSorulanSoruKategoriIndexVm
    {
        public SikcaSorulanSoruKategoriVm Kategori { get; set; } = new();
        public List<GetSikcaSorulanSoruKategoriVm> Kategoriler { get; set; } = new();
    }
}