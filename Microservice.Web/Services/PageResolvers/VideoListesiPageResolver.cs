using Microservice.Web.Clients.DuyuruClients;
using Microservice.Web.Clients.VideoClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Settings;
using Microservice.Web.ViewModels.PageRoute;

namespace Microservice.Web.Services.PageResolvers
{
    public class VideoListesiPageResolver : IPageResolver
    {
        private readonly IVideoClientServices _videoClient;
        private readonly ILogger<VideoListesiPageResolver> _logger;

        public VideoListesiPageResolver(
            IVideoClientServices videoClient,
            ILogger<VideoListesiPageResolver> logger)
        {
            _videoClient = videoClient;
            _logger = logger;
        }

        public bool CanResolve(PageTypeKindEnum pageType)
        {
            return pageType == PageTypeKindEnum.VideoListesi;
        }

        public async Task ResolveAsync(RouteResolveResult result)
        {
            var response = await _videoClient.GetVideolarAsync(result.Site.Id, result.LanguageId);

            if (!response.IsSuccessful || response.Content is null)
            {
                _logger.LogWarning(
                    "Video listesi bulunamadı. SiteId: {SiteId}, LanguageId: {LanguageId}",
                    result.Site.Id,
                    result.LanguageId);

                return;
            }

            result.VideoListesi = response.Content;
        }
    }
}
