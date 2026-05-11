using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.BilgiFeatures.DeleteBilgi
{
    public class DeleteBilgiCommandHandler(
          IBilgiRepository bilgiRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<DeleteBilgiCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteBilgiCommand request, CancellationToken cancellationToken)
        {
            var bilgi = await bilgiRepository.GetByIdAsync(request.Id);
            if (bilgi == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            bilgi.IsDeleted = true;
            bilgiRepository.Update(bilgi);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var cacheKey = $"bilgis:list:{bilgi.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
