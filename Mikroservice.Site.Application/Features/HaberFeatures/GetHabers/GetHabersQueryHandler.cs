using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.HaberFeatures.GetHabers
{
    public class GetHabersQueryHandler(
          IHaberRepository haberRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetHabersQueryHandler> logger
        )
        : IRequestHandler<GetHabersQuery, ServiceResult<List<Haber>>>
    {
        public async Task<ServiceResult<List<Haber>>> Handle(GetHabersQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"habers:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<Haber>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                "Haber cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                cached.Count);
                return ServiceResult<List<Haber>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = await haberRepository.GetAll().Where(b => b.SiteId == request.SiteId && b.DilId == request.DilId).ToListAsync(cancellationToken);

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation(
                "Haber verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);
            return ServiceResult<List<Haber>>.SuccessAsOK(data);
        }
    }
}
