using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microservice.Site.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Microservice.Site.Application.Features.YonetimDuyuruFeatures.GetYonetimDuyurulari
{
    public class GetYonetimDuyurulariQueryHandler(
          IYonetimDuyuruRepository yonetimDuyuruRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetYonetimDuyurulariQueryHandler> logger
        )
        : IRequestHandler<GetYonetimDuyurulariQuery, ServiceResult<List<YonetimDuyuru>>>
    {
        public async Task<ServiceResult<List<YonetimDuyuru>>> Handle(GetYonetimDuyurulariQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = "list:yonetimDuyurulari";
            var cached = await redisCacheService.GetListAsync<YonetimDuyuru>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation("Yönetim duyuruları verisi cacheden alındı. Yönetim duyuruları sayısı: {@yonetimDuyuruCount}", cached.Count);
                return ServiceResult<List<YonetimDuyuru>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = yonetimDuyuruRepository.GetAll().ToList();

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation("Yönetim duyuruları verisi veritabanından alındı. Yönetim duyuruları sayısı:{@yonetimDuyuruCount}", data.Count);
            return ServiceResult<List<YonetimDuyuru>>.SuccessAsOK(data);
        }
    }
}
