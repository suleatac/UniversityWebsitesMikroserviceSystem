using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.BannerFeatures.DeleteBanner
{
    public class DeleteBannerCommandHandler(
          IBannerRepository bannerRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<DeleteBannerCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteBannerCommand request, CancellationToken cancellationToken)
        {
            var banner = await bannerRepository.GetByIdAsync(request.Id);
            if (banner == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            banner.IsDeleted = true;
            bannerRepository.Update(banner);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var cacheKey = $"banners:list:{banner.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
