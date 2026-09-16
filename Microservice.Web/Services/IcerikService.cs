using Microservice.Web.Clients.IcerikClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Icerik;
using Microservice.Web.ViewModels.Paged;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class IcerikService : IIcerikService
    {
        private readonly IIcerikClientServices _icerikClient;
        private readonly ILogger<IcerikService> _logger;

        public IcerikService(IIcerikClientServices icerikClient, ILogger<IcerikService> logger)
        {
            _icerikClient = icerikClient ?? throw new ArgumentNullException(nameof(icerikClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<PagedResultVm<IcerikSearchVm>>> SearchAsync(
            int siteId,
            int dilId,
            string? search,
            int? tip,
            int page,
            int pageSize)
        {
            _logger.LogInformation(
                "Icerik aramasi yapiliyor. SiteId: {SiteId}, DilId: {DilId}, Search: {Search}, Tip: {Tip}, Page: {Page}",
                siteId, dilId, search, tip, page);

            var response = await _icerikClient.SearchAsync(siteId, dilId, search, tip, page, pageSize);

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

                return ServiceResult<PagedResultVm<IcerikSearchVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Arama sonuclari alinamadi"
                );
            }

            var content = response.Content ?? new PagedResultVm<IcerikSearchVm>();

            _logger.LogInformation(
                "Arama sonucu alindi. TotalCount: {TotalCount}, Page: {Page}",
                content.TotalCount,
                content.Page);

            return ServiceResult<PagedResultVm<IcerikSearchVm>>.Success(content);
        }
    }
}
