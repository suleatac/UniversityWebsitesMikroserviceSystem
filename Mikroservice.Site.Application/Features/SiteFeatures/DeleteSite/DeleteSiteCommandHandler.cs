using MediatR;
using MassTransit;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SiteEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SiteFeatures.DeleteSite
{
    public class DeleteSiteCommandHandler(
     ISiteRepository siteRepository,
     IUnitOfWork unitOfWork,
     IRedisCacheService redisCache,
     IPublishEndpoint publishEndpoint
 ) : IRequestHandler<DeleteSiteCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteSiteCommand request, CancellationToken cancellationToken)
        {
            var site = await siteRepository.GetByIdAsync(request.Id);

            if (site == null || site.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            var siteAlanAdi = site.SiteAlanAdi;

            site.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Cache invalidation
            await redisCache.RemoveByPatternAsync(
                "site:*",
                cancellationToken);

            await publishEndpoint.Publish(
                new SiteChangedEvent(site.Id, siteAlanAdi, null, SiteChangeType.Deleted),
                cancellationToken);
            return ServiceResult.Success();
        }
    }
}
