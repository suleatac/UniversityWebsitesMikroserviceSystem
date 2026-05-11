using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.YonetimDuyuru;

namespace Microservice.Site.Application.Features.YonetimDuyuruFeatures.GetYonetimDuyurulari
{
    public class GetYonetimDuyurulariQueryHandler(
          IYonetimDuyuruRepository yonetimDuyuruRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetYonetimDuyurulariQueryHandler> logger,
          IMapper mapper
        )
        : IRequestHandler<GetYonetimDuyurulariQuery, ServiceResult<List<YonetimDuyuruDto>>>
    {
        public async Task<ServiceResult<List<YonetimDuyuruDto>>> Handle(GetYonetimDuyurulariQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = "yonetimduyuru:list";
            var cached = await redisCacheService.GetListAsync<YonetimDuyuruDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation("Yönetim duyuruları verisi cacheden alındı. Yönetim duyuruları sayısı: {@yonetimDuyuruCount}", cached.Count);
                return ServiceResult<List<YonetimDuyuruDto>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = yonetimDuyuruRepository.GetAll().ToList();
            var mappedData = mapper.Map<List<YonetimDuyuruDto>>(data);
            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, mappedData, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation("Yönetim duyuruları verisi veritabanından alındı. Yönetim duyuruları sayısı:{@yonetimDuyuruCount}", mappedData.Count);
            return ServiceResult<List<YonetimDuyuruDto>>.SuccessAsOK(mappedData);
        }
    }
}
