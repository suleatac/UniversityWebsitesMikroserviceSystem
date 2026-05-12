using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.HaberFeatures.DeleteHaber
{
    public class DeleteHaberCommandHandler(
          IHaberRepository haberRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<DeleteHaberCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteHaberCommand request, CancellationToken cancellationToken)
        {
            var haber = await haberRepository.GetByIdAsync(request.Id);
            if (haber == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            haber.IsDeleted = true;
            haberRepository.Update(haber);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Cache silme
            var cacheKey = $"haber:list:{haber.SiteId}:*";
            await redisCache.RemoveByPatternAsync(
                cacheKey,
                cancellationToken);

            return ServiceResult.Success();
        }
    }
}
