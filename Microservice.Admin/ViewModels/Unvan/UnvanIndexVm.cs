namespace Microservice.Admin.ViewModels.Unvan
{
    public class UnvanIndexVm
    {
        public UnvanVm Unvan { get; set; } = new();
        public List<GetUnvanVm> Unvanlar { get; set; } = new();
    }
}
