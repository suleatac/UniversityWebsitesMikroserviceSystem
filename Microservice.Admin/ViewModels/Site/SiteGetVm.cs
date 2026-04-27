namespace Microservice.Admin.ViewModels.Site
{
    public class SiteGetVm
    { 
            public int Id { get; set; }
            public string SiteAdi { get; set; } = default!;
            public string SiteUrl { get; set; } = default!;
            public string SiteEPosta { get; set; } = default!;     
    }
}
