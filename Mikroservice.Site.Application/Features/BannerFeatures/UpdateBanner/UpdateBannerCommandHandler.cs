using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.BannerFeatures.UpdateBanner
{
    public class UpdateBannerCommandHandler(
          IBannerRepository bannerRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<UpdateBannerCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateBannerCommand request, CancellationToken cancellationToken)
        {
            var banner = await bannerRepository.GetByIdAsync(request.Id);
            if (banner == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            banner.Baslik = request.Baslik;
            banner.KisaAciklama = request.KisaAciklama;
            banner.IcerikMetni = request.IcerikMetni;
            banner.Link = request.Link;
            banner.ResimUrl = request.ResimUrl;
            banner.Sira = request.Sira;
            banner.YayimTarihi = request.YayimTarihi;
            banner.BaslamaTarihi = request.BaslamaTarihi;
            banner.BitisTarihi = request.BitisTarihi;
            banner.SeoUrl = request.SeoUrl;
            banner.SeoTitle = request.SeoTitle;
            banner.SeoDescription = request.SeoDescription;
            banner.SiteId = request.SiteId;
            banner.DilId = request.DilId;
            banner.HedefId = request.HedefId;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var cacheKey = $"banners:list:{banner.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);


            return ServiceResult.Success();
        }
    }
}
