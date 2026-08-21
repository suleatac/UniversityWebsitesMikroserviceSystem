using Microservice.Web.Clients.HaberClients;
using Microservice.Web.Clients.PageRouteClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Haber;
using Microservice.Web.ViewModels.PageRoute;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class PageRouteService : IPageRouteService
    {
        private readonly IPageRouteClientServices _pagerouteClient;
        private readonly ILogger<PageRouteService> _logger;

        public PageRouteService(IPageRouteClientServices pagerouteClient, ILogger<PageRouteService> logger)
        {
            _pagerouteClient = pagerouteClient ?? throw new ArgumentNullException(nameof(pagerouteClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        // GET BY Slug
        public async Task<ServiceResult<PageRouteSlugDetailVm>> GetPageRouteBySlugAsync(int siteId, string slug)
        {
            _logger.LogInformation("Page route getiriliyor. SiteId: {SiteId}, Slug: {Slug}", siteId, slug);

            var response = await _pagerouteClient.GetPageRouteBySlugAsync(siteId, slug);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<PageRouteSlugDetailVm>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Page route bulunamadı"
                );
            }

            return ServiceResult<PageRouteSlugDetailVm>.Success(response.Content!);
        }



    }
}
