using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.PopupFeatures.GetPopup
{
    public class GetPopupQueryHandler(
          IPopupRepository popupRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetPopupQueryHandler> logger
        )
        : IRequestHandler<GetPopupQuery, ServiceResult<List<Popup>>>
    {
        public async Task<ServiceResult<List<Popup>>> Handle(GetPopupQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"popup:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<Popup>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                "Popup cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                cached.Count);
                return ServiceResult<List<Popup>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = await popupRepository.GetAll().Where(b => b.SiteId == request.SiteId && b.DilId == request.DilId).ToListAsync(cancellationToken);

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation(
                "Popup verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);
            return ServiceResult<List<Popup>>.SuccessAsOK(data);
        }
    }
}
