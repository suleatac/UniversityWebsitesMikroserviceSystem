using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Yonetici.Application.Contracts.IRepositories;
using Microservice.Yonetici.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Microservice.Yonetici.Application.Features.YoneticiDuyuruFeatures.GetYoneticiDuyurulari
{
    public class GetYoneticiDuyurulariQueryHandler(
          IYoneticiDuyuruRepository yoneticiDuyuruRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetYoneticiDuyurulariQueryHandler> logger
        )
        : IRequestHandler<GetYoneticiDuyurulariQuery, ServiceResult<List<YoneticiDuyuru>>>
    {
        public async Task<ServiceResult<List<YoneticiDuyuru>>> Handle(GetYoneticiDuyurulariQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = "list:yoneticiDuyurulari";
            var cached = await redisCacheService.GetListAsync<YoneticiDuyuru>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation("Yönetici duyuruları verisi cacheden alındı. Yönetici duyuruları sayısı: {@yoneticiDuyuruCount}", cached.Count);
                return ServiceResult<List<YoneticiDuyuru>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = yoneticiDuyuruRepository.GetAll().ToList();

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation("Yönetici duyuruları verisi veritabanından alındı. Yönetici duyuruları sayısı:{@yoneticiDuyuruCount}", data.Count);
            return ServiceResult<List<YoneticiDuyuru>>.SuccessAsOK(data);
        }
    }
}
