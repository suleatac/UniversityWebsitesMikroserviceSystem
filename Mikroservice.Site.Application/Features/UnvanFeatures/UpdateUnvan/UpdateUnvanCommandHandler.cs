using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.UpdateUnvan
{
    public class UpdateUnvanCommandHandler(
     IUnvanRepository repository,
     IUnitOfWork unitOfWork,
     IRedisCacheService redisCache
 ) : IRequestHandler<UpdateUnvanCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateUnvanCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null)
                return ServiceResult.ErrorAsNotFound();

            entity.TipId = request.TipId;
            entity.Ad = request.Ad;
            entity.KisaAd = request.KisaAd;
            entity.Sira = request.Sira;
            entity.ParentId = request.ParentId;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Cache invalidation
            await redisCache.RemoveAsync(
                "unvan:list",
                cancellationToken);

            return ServiceResult.Success();
        }
    }
}
