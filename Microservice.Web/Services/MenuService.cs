using Microservice.Web.Clients.MenuClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Menu;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuClientServices _menuClient;
        private readonly ILogger<MenuService> _logger;

        public MenuService(IMenuClientServices menuClient, ILogger<MenuService> logger)
        {
            _menuClient = menuClient;
            _logger = logger;
        }

        public async Task<ServiceResult<List<MenuGetVm>>> GetMenusAsync(int siteId, int dilId)
        {
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

            return ServiceResult<List<MenuGetVm>>.Success(response.Content ?? new List<MenuGetVm>());
        }
    }
}