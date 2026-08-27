using Microservice.Web.Clients.MenuClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Menu;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class MenuService : IMenuService
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        private readonly IMenuClientServices _menuClient;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger<MenuService> _logger;

        public MenuService(IMenuClientServices menuClient, IRedisCacheService redisCacheService, ILogger<MenuService> logger)
        {
            _menuClient = menuClient;
            _redisCacheService = redisCacheService;
            _logger = logger;
        }

        public async Task<ServiceResult<List<MenuGetVm>>> GetMenusAsync(int siteId, int dilId)
        {
            var cacheKey = $"menu:list:{siteId}:{dilId}";

            var cached = await _redisCacheService.GetListAsync<MenuGetVm>(cacheKey);
            if (cached is not null)
            {
                _logger.LogInformation("Menuler cache'den al\u0131nd\u0131. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
                return ServiceResult<List<MenuGetVm>>.Success(cached);
            }

            _logger.LogInformation("Menuler çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);

            var response = await _menuClient.GetMenusAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError("Menuler alınamadı. StatusCode: {StatusCode}, Detail: {Detail}", response.StatusCode, problemDetails?.Detail);

                return ServiceResult<List<MenuGetVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Menuler alınamadı");
            }

            var menus = response.Content ?? new List<MenuGetVm>();
            await _redisCacheService.SetListAsync(cacheKey, menus, CacheDuration);

            return ServiceResult<List<MenuGetVm>>.Success(menus);
        }
    }
}