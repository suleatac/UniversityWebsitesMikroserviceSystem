using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.SiteDtos;

namespace Mikroservice.Site.Application.Features.SiteFeatures.GetSiteById
{
    public class GetSiteByIdQueryHandler(
    ISiteRepository siteRepository,
    IRedisCacheService redis,
    ILogger<GetSiteByIdQueryHandler> logger,
    IMapper mapper
) : IRequestHandler<GetSiteByIdQuery, ServiceResult<SiteDetailDto>>
    {
        public async Task<ServiceResult<SiteDetailDto>> Handle(GetSiteByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"site:{request.Id}";

            var cached = await redis.GetAsync<SiteDetailDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("Site cache'den alındı");

                var mappedCached = mapper.Map<SiteDetailDto>(cached);
                return ServiceResult<SiteDetailDto>.SuccessAsOK(mappedCached);
            }

            var data = siteRepository.GetAll()
                .Where(x => !x.IsDeleted)
                .ToList();

            await redis.SetListAsync(cacheKey, data, TimeSpan.FromHours(12), cancellationToken);

            var mappedData = mapper.Map<SiteDetailDto>(data);
            return ServiceResult<SiteDetailDto>.SuccessAsOK(mappedData);
        }
    }
}
