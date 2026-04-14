using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microservice.Site.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Microservice.Site.Application.Features.YoneticiDuyuruFeatures.GetYoneticiDuyurulari
{
    public class GetYoneticiDuyurulariQueryHandler(
          IYoneticiDuyuruRepository yoneticiDuyuruRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetYoneticiDuyurulariQueryHandler> logger
        )
        : IRequestHandler<GetYoneticiDuyurulariQuery, ServiceResult<List<YonetimDuyuru>>>
    {
        public async Task<ServiceResult<List<YonetimDuyuru>>> Handle(GetYoneticiDuyurulariQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = "list:yoneticiDuyurulari";
            var cached = await redisCacheService.GetListAsync<YonetimDuyuru>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation("Yönetici duyuruları verisi cacheden alındı. Yönetici duyuruları sayısı: {@yoneticiDuyuruCount}", cached.Count);
                return ServiceResult<List<YonetimDuyuru>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = yoneticiDuyuruRepository.GetAll().ToList();

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation("Yönetici duyuruları verisi veritabanından alındı. Yönetici duyuruları sayısı:{@yoneticiDuyuruCount}", data.Count);
            return ServiceResult<List<YonetimDuyuru>>.SuccessAsOK(data);
        }
    }
}
