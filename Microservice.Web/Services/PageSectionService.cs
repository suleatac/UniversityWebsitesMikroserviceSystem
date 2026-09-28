using Microservice.Web.Clients.PageSectionClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.PageSection;

namespace Microservice.Web.Services
{
    public class PageSectionService : IPageSectionService
    {
        private readonly IPageSectionClientServices _pageSectionClient;
        private readonly ILogger<PageSectionService> _logger;

        public PageSectionService(
            IPageSectionClientServices pageSectionClient,
            ILogger<PageSectionService> logger)
        {
            _pageSectionClient = pageSectionClient ?? throw new ArgumentNullException(nameof(pageSectionClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<GetPageSectionVm>>> GetPublishedSectionsAsync(int siteId, int dilId)
        {
            try
            {
                var response = await _pageSectionClient.GetPublishedPageSectionsAsync(siteId, dilId);

                if (!response.IsSuccessStatusCode || response.Content is null)
                {
                    _logger.LogWarning(
                        "Sayfa bolum verileri alinamadi. SiteId: {SiteId}, DilId: {DilId}, StatusCode: {StatusCode}",
                        siteId,
                        dilId,
                        response.StatusCode);

                    // Dinamik bolumler ana sayfa iceriginin zorunlu parcasi degil; bos liste ile devam edilir.
                    return ServiceResult<List<GetPageSectionVm>>.Success([]);
                }

                return ServiceResult<List<GetPageSectionVm>>.Success(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "PageSection servisi cagrisinda hata. SiteId: {SiteId}", siteId);
                return ServiceResult<List<GetPageSectionVm>>.Success([]);
            }
        }
    }
}
