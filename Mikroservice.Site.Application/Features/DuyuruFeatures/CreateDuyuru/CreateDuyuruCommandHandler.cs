using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.CreateDuyuru
{
    public class CreateDuyuruCommandHandler(
          IDuyuruRepository duyuruRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<CreateDuyuruCommand, ServiceResult<CreateDuyuruResponse>>
    {
        public async Task<ServiceResult<CreateDuyuruResponse>> Handle(CreateDuyuruCommand request, CancellationToken cancellationToken)
        {
            var newDuyuru = new Duyuru {
                Baslik = request.Baslik,
                KisaAciklama = request.KisaAciklama,
                IcerikMetni = request.IcerikMetni,
                Link = request.Link,
                ResimUrl = request.ResimUrl,
                YayimTarihi = request.YayimTarihi,
                BaslamaTarihi = request.BaslamaTarihi,
                BitisTarihi = request.BitisTarihi,
                SeoUrl = request.SeoUrl,
                SeoTitle = request.SeoTitle,
                SeoDescription = request.SeoDescription,
                SiteId = request.SiteId,
                DilId = request.DilId,
                HedefId = request.HedefId,

                EklemeTarihi = DateTime.Now,
                GosterimSayisi = 0,
                IsDeleted = false

            };
            await duyuruRepository.AddAsync(newDuyuru);
            await unitOfWork.SaveChangesAsync(cancellationToken);


            //Cache temizleme işlemi.
            var cacheKey = $"duyurus:list:{newDuyuru.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);

            var response = new CreateDuyuruResponse(newDuyuru.Id);
            return ServiceResult<CreateDuyuruResponse>
            .SuccessAsCreated(response, $"/api/v1/duyurus/{newDuyuru.Id}");
        }
    }
}
