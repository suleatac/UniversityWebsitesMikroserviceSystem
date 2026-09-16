using Microservice.Web.Clients.VideoClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageDetailResolvers
{
    public class VideoDetayResolver : IPageDetailResolver
    {
        private readonly IVideoClientServices _videoClient;
        private readonly ILogger<VideoDetayResolver> _logger;

        public VideoDetayResolver(
            IVideoClientServices videoClient,
            ILogger<VideoDetayResolver> logger)
        {
            _videoClient = videoClient;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.VideoDetay;
        }

        public async Task<RouteResolveResult?> ResolveAsync(
            RouteResolveResult result,
            string detailSlug)
        {
            var response = await _videoClient
                .GetVideoBySeoUrlAsync(
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Video bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}, SeoUrl: {SeoUrl}",
                    result.Site.Id,
                    result.LanguageId,
                    detailSlug);

                return null;
            }

            result.VideoDetay = response.Content;

            return result;
        }
    }
}
