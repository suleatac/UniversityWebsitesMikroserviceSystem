using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.DeleteSitePersonel
{
    public class DeleteSitePersonelCommandHandler(
      ISitePersonelRepository repository,
      IUnitOfWork unitOfWork,
      IRedisCacheService redisCache
  ) : IRequestHandler<DeleteSitePersonelCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteSitePersonelCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            entity.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var key = $"sitepersonel:list:{entity.SiteId}";
            await redisCache.RemoveAsync(key, cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
