using Microservice.Web.Clients.BannerClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Content;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public sealed class BannerService(IBannerClientServices client) : IBannerService
    {
        public async Task<ServiceResult<ContentDetailVm>> GetBannerByIdAsync(int id)
        {
            var response = await client.GetBannerByIdAsync(id);
            return ContentLookupService.ToResult(response, "Banner bulunamadı");
        }

        public async Task<ServiceResult<List<ContentDetailVm>>> GetBannersAsync(int siteId, int dilId)
        {
            var response = await client.GetBannersAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error?.Content is { } content
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(content)
                    : null;

                return ServiceResult<List<ContentDetailVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Banner listesi alınamadı");
            }

            return ServiceResult<List<ContentDetailVm>>.Success(response.Content ?? new List<ContentDetailVm>());
        }
    }
}
