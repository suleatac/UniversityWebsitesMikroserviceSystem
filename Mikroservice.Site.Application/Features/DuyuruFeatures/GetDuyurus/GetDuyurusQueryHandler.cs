using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.DuyuruDtos;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.GetDuyurus
{
    public class GetDuyurusQueryHandler(
          IDuyuruRepository duyuruRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetDuyurusQueryHandler> logger,
          IMapper mapper
        )
        : IRequestHandler<GetDuyurusQuery, ServiceResult<List<DuyuruDto>>>
    {
        public async Task<ServiceResult<List<DuyuruDto>>> Handle(GetDuyurusQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"duyuru:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<DuyuruDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                "Duyuru cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                cached.Count);
                return ServiceResult<List<DuyuruDto>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = await duyuruRepository.GetBySiteAndLanguageAsync(request.SiteId, request.DilId, cancellationToken);

            //Loglama
            logger.LogInformation(
                "Duyuru verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);

            var mappedData = mapper.Map<List<DuyuruDto>>(data);


            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, mappedData, TimeSpan.FromHours(24), cancellationToken);
    
            return ServiceResult<List<DuyuruDto>>.SuccessAsOK(mappedData);
        }
    }
}
