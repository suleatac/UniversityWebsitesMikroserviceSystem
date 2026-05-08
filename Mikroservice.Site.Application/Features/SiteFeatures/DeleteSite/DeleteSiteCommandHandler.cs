using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SiteFeatures.DeleteSite
{
    public class DeleteSiteCommandHandler(
     ISiteRepository siteRepository,
     IUnitOfWork unitOfWork,
     IRedisCacheService redisCache
 ) : IRequestHandler<DeleteSiteCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteSiteCommand request, CancellationToken cancellationToken)
        {
            var site = await siteRepository.GetByIdAsync(request.Id);

            if (site == null || site.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            site.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Cache invalidation
            await redisCache.RemoveByPatternAsync(
                "site:*",
                cancellationToken);
            return ServiceResult.SuccessAsNoContent();
        }
    }
}
