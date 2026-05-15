using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.BannerFeatures.CreateBanner
{
    public class CreateBannerCommandHandler(
          IBannerRepository bannerRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<CreateBannerCommand, ServiceResult<CreateBannerResponse>>
    {
        public async Task<ServiceResult<CreateBannerResponse>> Handle(CreateBannerCommand request, CancellationToken cancellationToken)
        {
            var newBanner = new Banner {
                Baslik = request.Baslik,
                KisaAciklama = request.KisaAciklama,
                IcerikMetni = request.IcerikMetni,
                Link = request.Link,
                ResimUrl = request.ResimUrl,
                Sira = request.Sira,
                YayimTarihi = request.YayimTarihi,
                BaslamaTarihi = request.BaslamaTarihi,
                BitisTarihi = request.BitisTarihi,
                SeoUrl = request.SeoUrl,
                SeoTitle = request.SeoTitle,
                SeoDescription = request.SeoDescription,
                SiteId = request.SiteId,
                DilId = request.DilId,
                HedefId=request.HedefId,

                EklemeTarihi = DateTime.Now,
                GosterimSayisi = 0,
                IsDeleted = false

            };
            await bannerRepository.AddAsync(newBanner);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var cacheKey = $"banners:list:{newBanner.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);

            var response = new CreateBannerResponse(newBanner.Id);
            return ServiceResult<CreateBannerResponse>
            .SuccessAsCreated(response, $"/api/v1/banners/{newBanner.Id}");
        }
    }
}
