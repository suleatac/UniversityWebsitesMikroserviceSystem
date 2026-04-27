using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.SiteDtos;

namespace Mikroservice.Site.Application.Features.SiteFeatures.GetSite
{
    public class GetSiteQueryHandler(
    ISiteRepository siteRepository,
    IRedisCacheService redis,
    ILogger<GetSiteQueryHandler> logger,
    IMapper mapper
) : IRequestHandler<GetSiteQuery, ServiceResult<List<SiteDto>>>
    {
        public async Task<ServiceResult<List<SiteDto>>> Handle(GetSiteQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "site:list";

            var cached = await redis.GetListAsync<SiteDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("Site cache'den alındı");

                var mappedCached = mapper.Map<List<SiteDto>>(cached);
                return ServiceResult<List<SiteDto>>.SuccessAsOK(mappedCached);
            }

            var data = siteRepository.GetAll()
                .Where(x => !x.IsDeleted)
                .ToList();

            await redis.SetListAsync(cacheKey, data, TimeSpan.FromHours(12), cancellationToken);

            var mappedData = mapper.Map<List<SiteDto>>(data);
            return ServiceResult<List<SiteDto>>.SuccessAsOK(mappedData);
        }
    }
}
