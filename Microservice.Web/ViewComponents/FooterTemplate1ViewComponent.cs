using Microservice.Web.Services.Interfaces;
using Microservice.Web.ViewModels.BandLogo;
using Microservice.Web.ViewModels.Menu;
using Microservice.Web.ViewModels.Site;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Web.ViewComponents
{
    public class FooterTemplate1ViewComponent : ViewComponent
    {
        private readonly ILogger<FooterTemplate1ViewComponent> _logger;
        private readonly IBandLogoService _bandLogoService;
        public FooterTemplate1ViewComponent(ILogger<FooterTemplate1ViewComponent> logger, IBandLogoService bandLogoService)
        {
            _logger = logger;
            _bandLogoService = bandLogoService;
        }
        public async Task<IViewComponentResult> InvokeAsync(
           int siteId,
           int dilId,
           SiteDetailGetVm? preloadedSite = null)
        {
            if (siteId <= 0 || dilId <= 0)
            {
                return View(new MenuGetIndexVm());
            }

            var bandLogos = await _bandLogoService.GetBandLogosAsync(siteId, dilId);

           

            var site = preloadedSite ?? new SiteDetailGetVm {
                Id = siteId
            };

            var viewModel = new BandLogoGetIndexVm {
                Site = site,
                BandLogos = bandLogos.Data ?? new List<GetBandLogoVm>()
            };

            return View(viewModel);
        }
    }
}
