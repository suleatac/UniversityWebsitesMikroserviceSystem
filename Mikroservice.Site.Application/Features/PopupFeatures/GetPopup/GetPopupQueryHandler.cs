using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Mikroservice.Site.Application.Features.PopupFeatures.GetPopup
{
    public class GetPopupQueryHandler(
          IPopupRepository popupRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetPopupQueryHandler> logger
        )
        : IRequestHandler<GetPopupQuery, ServiceResult<Popup>>
    {
        public async Task<ServiceResult<Popup>> Handle(GetPopupQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"popup:site:{request.SiteId}";
            var cached = await redisCacheService.GetAsync<Popup>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation(
                "Popup cache'den alındı. SiteId:{siteId}",
                request.SiteId);
                return ServiceResult<Popup>.SuccessAsOK(cached);
            }

            var popup = await popupRepository.GetBySiteIdAsync(request.SiteId);
            if (popup is null)
            {
                logger.LogWarning("Popup bulunamadı. SiteId: {SiteId}", request.SiteId);
                return ServiceResult<Popup>.Error("Popup bulunamadı", System.Net.HttpStatusCode.NotFound);
            }

            await redisCacheService.SetAsync(cacheKey, popup, cancellationToken: cancellationToken);

            logger.LogInformation(
                "Popup verisi veritabanından alındı. SiteId:{siteId}",
                request.SiteId);
            return ServiceResult<Popup>.SuccessAsOK(popup);
        }
    }
}
