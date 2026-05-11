using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.UpdateYoneticiSite
{
    public class UpdateYoneticiSiteCommandHandler(
    IYoneticiSiteRepository repository,
    IUnitOfWork unitOfWork,
    IRedisCacheService redisCache
) : IRequestHandler<UpdateYoneticiSiteCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateYoneticiSiteCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            entity.YoneticiTipiId = request.YoneticiTipiId;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var key = "yoneticiSite:list:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
