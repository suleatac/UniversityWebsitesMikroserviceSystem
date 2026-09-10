using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.BandLogo
{
    public class BandLogoGetIndexVm
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        public List<GetBandLogoVm> BandLogos { get; set; } = new();
    }
}
