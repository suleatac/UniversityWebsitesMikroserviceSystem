using System.Text.Json;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Content;

namespace Microservice.Web.Services
{
    internal static class ContentLookupService
    {
        public static ServiceResult<ContentDetailVm> ToResult(
            Refit.ApiResponse<ContentDetailVm> response,
            string notFoundMessage)
        {
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                return ServiceResult<ContentDetailVm>.Success(response.Content);
            }

            var problemDetails = response.Error?.Content is { } content
                ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(content)
                : null;

            return ServiceResult<ContentDetailVm>.Error(
                problemDetails?.Detail ?? problemDetails?.Title ?? notFoundMessage);
        }
    }
}
