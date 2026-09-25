using Microservice.Web.Clients.PopupClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Popup;

namespace Microservice.Web.Services
{
    public class PopupService : IPopupService
    {
        private readonly IPopupClientServices _popupClient;
        private readonly ILogger<PopupService> _logger;

        public PopupService(IPopupClientServices popupClient, ILogger<PopupService> logger)
        {
            _popupClient = popupClient ?? throw new ArgumentNullException(nameof(popupClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<GetPopupVm?>> GetPopupAsync(int siteId)
        {
            try
            {
                var response = await _popupClient.GetPopupBySiteIdAsync(siteId);

                // Site API popup kaydi olmadiginda 404 dondurur; bu bir hata degil "popup yok" demektir.
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return ServiceResult<GetPopupVm?>.Success(null);
                }

                if (!response.IsSuccessStatusCode || response.Content is null)
                {
                    _logger.LogWarning(
                        "Popup bilgisi alinamadi. SiteId: {SiteId}, StatusCode: {StatusCode}",
                        siteId,
                        response.StatusCode);

                    // Popup ana sayfa iceriginin zorunlu parcasi degil; hata durumunda logosuz devam edilir.
                    return ServiceResult<GetPopupVm?>.Success(null);
                }

                return ServiceResult<GetPopupVm?>.Success(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Popup servisi cagrisinda hata. SiteId: {SiteId}", siteId);
                return ServiceResult<GetPopupVm?>.Success(null);
            }
        }
    }
}
