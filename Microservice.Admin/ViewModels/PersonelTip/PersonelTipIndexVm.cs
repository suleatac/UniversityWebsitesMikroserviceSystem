namespace Microservice.Admin.ViewModels.PersonelTip
{
    public class PersonelTipIndexVm
    {
        public PersonelTipVm PersonelTip { get; set; } = new();
        public List<GetPersonelTipVm> PersonelTipler { get; set; } = new();
    }
}