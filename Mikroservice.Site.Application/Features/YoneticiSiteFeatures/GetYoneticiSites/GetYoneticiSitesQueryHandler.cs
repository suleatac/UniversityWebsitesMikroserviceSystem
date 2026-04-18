using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.GetYoneticiSites
{
    public class GetYoneticiSitesQueryHandler(
      IYoneticiSiteRepository repository,
      IRedisCacheService redis,
      ILogger<GetYoneticiSitesQueryHandler> logger
  ) : IRequestHandler<GetYoneticiSitesQuery, ServiceResult<List<YoneticiSite>>>
    {
        public async Task<ServiceResult<List<YoneticiSite>>> Handle(
            GetYoneticiSitesQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = $"yoneticiSite:list:{request.SiteId}";

            var cached = await redis.GetListAsync<YoneticiSite>(cacheKey, cancellationToken);

            if (cached is not null)
            {
                logger.LogInformation("YoneticiSite cache'den alındı");
                return ServiceResult<List<YoneticiSite>>.SuccessAsOK(cached);
            }

            var data = repository.GetAll()
                .Where(x => x.SiteId == request.SiteId && !x.IsDeleted)
                .ToList();

            await redis.SetListAsync(cacheKey, data, TimeSpan.FromHours(12), cancellationToken);

            return ServiceResult<List<YoneticiSite>>.SuccessAsOK(data);
        }
    }
}
