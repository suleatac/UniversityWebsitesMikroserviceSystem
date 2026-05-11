using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.DeleteYoneticiSite
{
    public class DeleteYoneticiSiteCommandHandler(
    IYoneticiSiteRepository repository,
    IUnitOfWork unitOfWork,
    IRedisCacheService redisCache
) : IRequestHandler<DeleteYoneticiSiteCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteYoneticiSiteCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            entity.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var key = "yoneticiSite:list:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
