using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.BilgiFeatures.GetBilgis
{
    public class GetBilgisQueryHandler(
          IBilgiRepository bilgiRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetBilgisQueryHandler> logger
        )
        : IRequestHandler<GetBilgisQuery, ServiceResult<List<Bilgi>>>
    {
        public async Task<ServiceResult<List<Bilgi>>> Handle(GetBilgisQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"bilgis:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<Bilgi>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                "Bilgi cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                cached.Count);
                return ServiceResult<List<Bilgi>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = bilgiRepository.GetAll().Where(b => b.SiteId == request.SiteId && b.DilId == request.DilId).OrderBy(x => x.Sira).ToList();

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation(
                "Bilgi verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);
            return ServiceResult<List<Bilgi>>.SuccessAsOK(data);
        }
    }
}
