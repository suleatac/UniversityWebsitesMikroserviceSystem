using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.GetSiteOzellikleri
{
    public class GetSiteOzellikleriQueryHandler(
      ISiteOzellikleriRepository repository,
      IRedisCacheService redis
  ) : IRequestHandler<GetSiteOzellikleriQuery, ServiceResult<SiteOzellikleri>>
    {
        public async Task<ServiceResult<SiteOzellikleri>> Handle(GetSiteOzellikleriQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"siteozellikleri:{request.SiteId}";

            var cached = await redis.GetAsync<SiteOzellikleri>(cacheKey, cancellationToken);

            if (cached != null)
                return ServiceResult<SiteOzellikleri>.SuccessAsOK(cached);

            var data = await repository.Where(x => x.SiteId == request.SiteId).FirstOrDefaultAsync(cancellationToken);

            if (data == null)
                return ServiceResult<SiteOzellikleri>.Error("Site özellikleri bulunamadı.", System.Net.HttpStatusCode.NotFound);

            await redis.SetAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            return ServiceResult<SiteOzellikleri>.SuccessAsOK(data);
        }
    }
}
