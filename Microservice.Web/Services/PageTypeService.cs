using Microservice.Web.Clients.PageTypeClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Pages;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class PageTypeService : IPageTypeService
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        private readonly IPageTypeClientServices _pageTypeClient;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger<PageTypeService> _logger;

        public PageTypeService(
            IPageTypeClientServices pageTypeClient,
            IRedisCacheService redisCacheService,
            ILogger<PageTypeService> logger)
        {
            _pageTypeClient = pageTypeClient ?? throw new ArgumentNullException(nameof(pageTypeClient));
            _redisCacheService = redisCacheService ?? throw new ArgumentNullException(nameof(redisCacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<PagesDetailVm>> GetPageTypeByKindAsync(int templateId, int dilId, int pageTypeKind)
        {
            var cacheKey = $"pagetype:kind:{templateId}:{dilId}:{pageTypeKind}";

            var cached = await _redisCacheService.GetAsync<PagesDetailVm>(cacheKey);
            if (cached is not null)
            {
                return ServiceResult<PagesDetailVm>.Success(cached);
            }

            var response = await _pageTypeClient.GetPageTypeByKindAsync(templateId, dilId, pageTypeKind);

            if (!response.IsSuccessStatusCode || response.Content is null)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogWarning(
                    "PageType by-kind alinamadi. StatusCode: {StatusCode}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Detail);

                return ServiceResult<PagesDetailVm>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Sayfa tipi bulunamadi");
            }

            await _redisCacheService.SetAsync(cacheKey, response.Content, CacheDuration);

            return ServiceResult<PagesDetailVm>.Success(response.Content);
        }
    }
}
