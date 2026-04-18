using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;

namespace Mikroservice.Site.Application.Features.SiteFeatures.GetSite
{
    public class GetSiteQueryHandler(
    ISiteRepository siteRepository,
    IRedisCacheService redis,
    ILogger<GetSiteQueryHandler> logger
) : IRequestHandler<GetSiteQuery, ServiceResult<List<Domain.Entities.Site>>>
    {
        public async Task<ServiceResult<List<Domain.Entities.Site>>> Handle(GetSiteQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "site:list";

            var cached = await redis.GetListAsync<Domain.Entities.Site>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("Site cache'den alındı");
                return ServiceResult<List<Domain.Entities.Site>>.SuccessAsOK(cached);
            }

            var data = siteRepository.GetAll()
                .Where(x => !x.IsDeleted)
                .ToList();

            await redis.SetListAsync(cacheKey, data, TimeSpan.FromHours(12), cancellationToken);

            return ServiceResult<List<Domain.Entities.Site>>.SuccessAsOK(data);
        }
    }
}
