using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.GetDuyurus
{
    public class GetDuyurusQueryHandler(
          IDuyuruRepository duyuruRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetDuyurusQueryHandler> logger
        )
        : IRequestHandler<GetDuyurusQuery, ServiceResult<List<Duyuru>>>
    {
        public async Task<ServiceResult<List<Duyuru>>> Handle(GetDuyurusQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"duyurus:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<Duyuru>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                "Duyuru cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                cached.Count);
                return ServiceResult<List<Duyuru>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = duyuruRepository.GetAll().Where(b => b.SiteId == request.SiteId && b.DilId == request.DilId).OrderBy(x => x.Sira).ToList();

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation(
                "Duyuru verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);
            return ServiceResult<List<Duyuru>>.SuccessAsOK(data);
        }
    }
}
