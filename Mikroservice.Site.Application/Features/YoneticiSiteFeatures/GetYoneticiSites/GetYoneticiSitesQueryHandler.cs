using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.YoneticiSiteDtos;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.GetYoneticiSites
{
    public class GetYoneticiSitesQueryHandler(
      IYoneticiSiteRepository repository,
      IRedisCacheService redis,
      ILogger<GetYoneticiSitesQueryHandler> logger,
      IMapper mapper
  ) : IRequestHandler<GetYoneticiSitesQuery, ServiceResult<List<YoneticiSiteDetailDto>>>
    {
        public async Task<ServiceResult<List<YoneticiSiteDetailDto>>> Handle(
            GetYoneticiSitesQuery request,
            CancellationToken cancellationToken)
        {
            // yoneticiSite:list-v2: YoneticiSiteDetailDto formatında SiteAdi Include edilmiş veri
            var cacheKey = "yoneticiSite:list-v2";

            var cached = await redis.GetListAsync<YoneticiSiteDetailDto>(cacheKey, cancellationToken);

            if (cached is not null)
            {
                logger.LogInformation("YoneticiSite cache'den alındı");
                return ServiceResult<List<YoneticiSiteDetailDto>>.SuccessAsOK(cached);
            }

            var data = await repository.GetAllWithSiteAsync(cancellationToken);
            var mappedData = mapper.Map<List<YoneticiSiteDetailDto>>(data);
            await redis.SetListAsync(cacheKey, mappedData, TimeSpan.FromHours(12), cancellationToken);

            return ServiceResult<List<YoneticiSiteDetailDto>>.SuccessAsOK(mappedData);
        }
    }
}
