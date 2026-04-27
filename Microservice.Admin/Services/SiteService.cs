using Microservice.Admin.Clients.SiteClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Site;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class SiteService(ISiteClientServices siteRefitService, ILogger<SiteService> logger): ISiteService
    {
        public async Task<ServiceResult<List<SiteGetVm>>> GetSitesAsync()
        {
            var response = await siteRefitService.GetSitesAsync();
            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!);
                logger.LogError("Error retrieving sites: {StatusCode} - {Title} - {Detail}",
                    response.Error.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail);
                return ServiceResult<List<SiteGetVm>>.Error("Fail to retrieve sites. Please try again later.");
            }


            return ServiceResult<List<SiteGetVm>>.Success(response.Content!);

        }
    }
}
