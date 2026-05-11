using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.DeleteUnvan
{
    public class DeleteUnvanCommandHandler(
        IUnvanRepository repository,
        ISitePersonelRepository sitePersonelRepository,
        IUnitOfWork unitOfWork,
        IRedisCacheService redisCache
    ) : IRequestHandler<DeleteUnvanCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteUnvanCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null)
                return ServiceResult.ErrorAsNotFound();

            // 🔥 kullanım kontrolü
            var isUsed = await sitePersonelRepository.AnyByUnvanIdAsync(request.Id, cancellationToken);

            if (isUsed)
                return ServiceResult.Error("Bu ünvan kullanılıyor, silinemez.",System.Net.HttpStatusCode.Conflict);

            repository.Delete(entity);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Cache invalidation
            await redisCache.RemoveAsync(
                "unvan:list",
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
