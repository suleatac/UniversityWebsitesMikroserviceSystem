using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.GetSikcaSorulanSoru
{
    public class GetSikcaSorulanSoruQueryHandler(
          ISikcaSorulanSoruRepository popupRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetSikcaSorulanSoruQueryHandler> logger
        )
        : IRequestHandler<GetSikcaSorulanSoruQuery, ServiceResult<List<SikcaSorulanSoru>>>
    {
        public async Task<ServiceResult<List<SikcaSorulanSoru>>> Handle(GetSikcaSorulanSoruQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"sikcasorulansoru:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<SikcaSorulanSoru>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                "Sıkça Sorulan Soru cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                cached.Count);
                return ServiceResult<List<SikcaSorulanSoru>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = popupRepository.GetAll().Where(b => b.SiteId == request.SiteId && b.DilId == request.DilId).OrderBy(x => x.Sira).ToList();

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation(
                "Sıkça Sorulan Soru verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);
            return ServiceResult<List<SikcaSorulanSoru>>.SuccessAsOK(data);
        }
    }
}
