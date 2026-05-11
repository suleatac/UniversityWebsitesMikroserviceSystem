using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.DeleteDuyuru
{
    public class DeleteDuyuruCommandHandler(
          IDuyuruRepository duyuruRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<DeleteDuyuruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteDuyuruCommand request, CancellationToken cancellationToken)
        {
            var duyuru = await duyuruRepository.GetByIdAsync(request.Id);
            if (duyuru == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            duyuru.IsDeleted = true;
            duyuruRepository.Update(duyuru);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var cacheKey = $"duyurus:list:{duyuru.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
