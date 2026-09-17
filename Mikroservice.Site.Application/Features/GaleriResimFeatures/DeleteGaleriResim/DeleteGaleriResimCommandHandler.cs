using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.GaleriResimFeatures.DeleteGaleriResim
{
    public class DeleteGaleriResimCommandHandler(
        IGaleriResimRepository repository,
        IUnitOfWork unitOfWork,
        IRedisCacheService redisCache
    ) : IRequestHandler<DeleteGaleriResimCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteGaleriResimCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            entity.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var key = $"galeriresim:list:{entity.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
