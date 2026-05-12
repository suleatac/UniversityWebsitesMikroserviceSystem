using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.DeleteEtkinlik
{
    public class DeleteEtkinlikCommandHandler(
          IEtkinlikRepository etkinlikRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<DeleteEtkinlikCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteEtkinlikCommand request, CancellationToken cancellationToken)
        {
            var etkinlik = await etkinlikRepository.GetByIdAsync(request.Id);
            if (etkinlik == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            etkinlik.IsDeleted = true;
            etkinlikRepository.Update(etkinlik);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var cacheKey = $"etkinliks:list:{etkinlik.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
