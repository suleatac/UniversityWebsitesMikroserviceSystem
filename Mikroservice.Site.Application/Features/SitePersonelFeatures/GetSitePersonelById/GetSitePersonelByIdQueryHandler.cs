using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;
using System.Net;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.GetSitePersonelById
{
    public class GetSitePersonelByIdQueryHandler(
        ISitePersonelRepository sitePersonelRepository,
        IRedisCacheService redisCacheService,
        ILogger<GetSitePersonelByIdQueryHandler> logger
      )
      : IRequestHandler<GetSitePersonelByIdQuery, ServiceResult<SitePersonel>>
    {
        public async Task<ServiceResult<SitePersonel>> Handle(GetSitePersonelByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"site-personel:{request.Id}";

            var cached = await redisCacheService.GetAsync<SitePersonel>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("SitePersonel cache'den alındı. Id: {Id}", request.Id);
                return ServiceResult<SitePersonel>.SuccessAsOK(cached);
            }

            var entity = await sitePersonelRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("SitePersonel bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<SitePersonel>.Error("SitePersonel bulunamadı", HttpStatusCode.NotFound);
            }

            await redisCacheService.SetAsync(cacheKey, entity, TimeSpan.FromHours(24), cancellationToken);

            logger.LogInformation("SitePersonel DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<SitePersonel>.SuccessAsOK(entity);
        }
    }
}