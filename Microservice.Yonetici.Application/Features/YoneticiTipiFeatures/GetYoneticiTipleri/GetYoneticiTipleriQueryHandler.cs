using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Yonetici.Application.Contracts.IRepositories;
using Microservice.Yonetici.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Microservice.Yonetici.Application.Features.YoneticiTipiFeatures.GetYoneticiTipleri
{
    public class GetYoneticiTipleriQueryHandler
        (
          IYoneticiTipiRepository yoneticiTipiRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetYoneticiTipleriQueryHandler> logger
        )
        : IRequestHandler<GetYoneticiTipleriQuery, ServiceResult<List<YoneticiTipi>>>
    {
        public async Task<ServiceResult<List<YoneticiTipi>>> Handle(GetYoneticiTipleriQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = "list:yoneticiTipleri";
            var cached = await redisCacheService.GetListAsync<YoneticiTipi>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation("Yönetici tipleri verisi cacheden alındı. Yönetici tipi sayısı: {@yoneticiTipiCount}", cached.Count);
                return ServiceResult<List<YoneticiTipi>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = yoneticiTipiRepository.GetAll().ToList();

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);


            //Örnek Loglama
            logger.LogInformation("Yönetici tipleri verisi veritabanından alındı. Yönetici tipi sayısı:{@yoneticiTipiCount}", data.Count);
            return ServiceResult<List<YoneticiTipi>>.SuccessAsOK(data);
        }
    }
}
