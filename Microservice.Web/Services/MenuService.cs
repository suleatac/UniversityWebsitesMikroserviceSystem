using Microservice.Web.Clients.MenuClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Duyuru;
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

        public async Task<ServiceResult<List<MenuGetVm>>> GetMenusAsync(int siteId, int dilId, int? location = null)
        {
            var cacheKey = $"menu:list:{siteId}:{dilId}:{location?.ToString() ?? "all"}";

            //var cached = await _redisCacheService.GetListAsync<MenuGetVm>(cacheKey);
            //if (cached is not null)
            //{
            //    _logger.LogInformation("Menuler cache'den al\u0131nd\u0131. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            //    return ServiceResult<List<MenuGetVm>>.Success(cached);
            //}

            _logger.LogInformation("Menuler çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);

            var response = await _menuClient.GetMenusAsync(siteId, dilId, location);

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

        public async Task<ServiceResult<MenuDetailVm>> GetMenuByIdAsync(int id)
        {
            _logger.LogInformation("Menü getiriliyor. Id: {Id}", id);
            var response = await _menuClient.GetMenuByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<MenuDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Menü bulunamadı");
            }

            return ServiceResult<MenuDetailVm>.Success(response.Content!);
        }
    }
}