using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.EtkinlikDtos;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.GetEtkinliks
{
    public class GetEtkinliksQueryHandler(
          IEtkinlikRepository etkinlikRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetEtkinliksQueryHandler> logger,
          IMapper mapper
        )
        : IRequestHandler<GetEtkinliksQuery, ServiceResult<List<EtkinlikDto>>>
    {
        public async Task<ServiceResult<List<EtkinlikDto>>> Handle(GetEtkinliksQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"etkinlik:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<EtkinlikDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Loglama
                logger.LogInformation(
                "Etkinlik cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                cached.Count);
                return ServiceResult<List<EtkinlikDto>>.SuccessAsOK(cached);
            }



            // Yoksa veritabanından çek
            var data = await etkinlikRepository.GetBySiteAndLanguageAsync(request.SiteId, request.DilId, cancellationToken);

            //Loglama
            logger.LogInformation(
                "Etkinlik verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);

            var mappedData = mapper.Map<List<EtkinlikDto>>(data);

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, mappedData, TimeSpan.FromHours(24), cancellationToken);

            
            return ServiceResult<List<EtkinlikDto>>.SuccessAsOK(mappedData);
        }
    }
}
