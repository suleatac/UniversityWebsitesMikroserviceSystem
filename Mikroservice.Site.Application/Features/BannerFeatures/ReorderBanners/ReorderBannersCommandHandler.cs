using System.Net;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.BannerFeatures.ReorderBanners
{
    public sealed class ReorderBannersCommandHandler(
        IBannerRepository bannerRepository,
        IUnitOfWork unitOfWork,
        IRedisCacheService redisCache
    ) : IRequestHandler<ReorderBannersCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(ReorderBannersCommand request, CancellationToken cancellationToken)
        {
            if (request.Items == null || request.Items.Count == 0)
                return ServiceResult.Error("Geçersiz istek", "Sıralama için öğe listesi boş.", HttpStatusCode.BadRequest);

            foreach (var item in request.Items)
            {
                var banner = await bannerRepository.GetByIdAsync(item.Id);
                if (banner == null) continue;

                banner.Sira = item.Sira;
                bannerRepository.Update(banner);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Cache temizleme
            await redisCache.RemoveByPatternAsync("banners:list:*", cancellationToken);

            return ServiceResult.Success();
        }
    }
}
