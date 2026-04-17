using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.GetEtkinliks
{
    public class GetEtkinliksQueryHandler(
          IEtkinlikRepository etkinlikRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetEtkinliksQueryHandler> logger
        )
        : IRequestHandler<GetEtkinliksQuery, ServiceResult<List<Etkinlik>>>
    {
        public async Task<ServiceResult<List<Etkinlik>>> Handle(GetEtkinliksQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"etkinliks:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<Etkinlik>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                "Etkinlik cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                cached.Count);
                return ServiceResult<List<Etkinlik>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = etkinlikRepository.GetAll().Where(b => b.SiteId == request.SiteId && b.DilId == request.DilId).OrderBy(x => x.Sira).ToList();

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation(
                "Etkinlik verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);
            return ServiceResult<List<Etkinlik>>.SuccessAsOK(data);
        }
    }
}
