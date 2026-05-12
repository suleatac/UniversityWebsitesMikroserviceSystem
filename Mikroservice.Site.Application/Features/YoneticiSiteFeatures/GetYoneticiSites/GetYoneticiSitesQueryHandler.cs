using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.SiteDtos;
using Mikroservice.Site.Application.DTOs.YoneticiSiteDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.GetYoneticiSites
{
    public class GetYoneticiSitesQueryHandler(
      IYoneticiSiteRepository repository,
      IRedisCacheService redis,
      ILogger<GetYoneticiSitesQueryHandler> logger,
      IMapper mapper
  ) : IRequestHandler<GetYoneticiSitesQuery, ServiceResult<List<YoneticiSiteDto>>>
    {
        public async Task<ServiceResult<List<YoneticiSiteDto>>> Handle(
            GetYoneticiSitesQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = "yoneticiSite:list";

            var cached = await redis.GetListAsync<YoneticiSiteDto>(cacheKey, cancellationToken);

            if (cached is not null)
            {
                logger.LogInformation("YoneticiSite cache'den alındı");
                return ServiceResult<List<YoneticiSiteDto>>.SuccessAsOK(cached);
            }

            var data = repository.GetAll().ToList();
            var mappedData = mapper.Map<List<YoneticiSiteDto>>(data);
            await redis.SetListAsync(cacheKey, mappedData, TimeSpan.FromHours(12), cancellationToken);

            return ServiceResult<List<YoneticiSiteDto>>.SuccessAsOK(mappedData);
        }
    }
}
