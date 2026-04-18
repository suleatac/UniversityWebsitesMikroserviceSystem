using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.GetSitePersonels
{
    public class GetSitePersonelQueryHandler(
       ISitePersonelRepository repository,
       IRedisCacheService redis,
       ILogger<GetSitePersonelQueryHandler> logger
   ) : IRequestHandler<GetSitePersonelQuery, ServiceResult<List<SitePersonel>>>
    {
        public async Task<ServiceResult<List<SitePersonel>>> Handle(
            GetSitePersonelQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = $"sitepersonel:list:{request.SiteId}";

            var cached = await redis.GetListAsync<SitePersonel>(cacheKey, cancellationToken);

            if (cached is not null)
            {
                logger.LogInformation("SitePersonel cache'den alındı. SiteId:{siteId}", request.SiteId);
                return ServiceResult<List<SitePersonel>>.SuccessAsOK(cached);
            }

            var data = repository.GetAll()
                .Where(x => x.SiteId == request.SiteId && !x.IsDeleted)
                .ToList();

            await redis.SetListAsync(cacheKey, data, TimeSpan.FromHours(12), cancellationToken);

            return ServiceResult<List<SitePersonel>>.SuccessAsOK(data);
        }
    }
}
