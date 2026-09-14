using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.BilgiDtos;

namespace Mikroservice.Site.Application.Features.BilgiFeatures.GetBilgis
{
    public class GetBilgisQueryHandler(
          IBilgiRepository bilgiRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetBilgisQueryHandler> logger,
          IMapper mapper
        )
        : IRequestHandler<GetBilgisQuery, ServiceResult<List<BilgiDto>>>
    {
        public async Task<ServiceResult<List<BilgiDto>>> Handle(GetBilgisQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"bilgi:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<BilgiDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                "Bilgi cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                cached.Count);
                return ServiceResult<List<BilgiDto>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = await bilgiRepository.GetBySiteAndLanguageAsync(request.SiteId, request.DilId, cancellationToken);

            //Loglama
            logger.LogInformation(
                "Bilgi verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);

            var mappedData = mapper.Map<List<BilgiDto>>(data);
            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, mappedData, TimeSpan.FromHours(24), cancellationToken);

      
            return ServiceResult<List<BilgiDto>>.SuccessAsOK(mappedData);
        }
    }
}
