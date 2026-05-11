using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.CreateEtkinlik
{
    public class CreateEtkinlikCommandHandler(
          IEtkinlikRepository etkinlikRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<CreateEtkinlikCommand, ServiceResult<CreateEtkinlikResponse>>
    {
        public async Task<ServiceResult<CreateEtkinlikResponse>> Handle(CreateEtkinlikCommand request, CancellationToken cancellationToken)
        {
            var newEtkinlik = new Etkinlik {
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
            await etkinlikRepository.AddAsync(newEtkinlik);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var cacheKey = $"etkinliks:list:{newEtkinlik.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);

            var response = new CreateEtkinlikResponse(newEtkinlik.Id);
            return ServiceResult<CreateEtkinlikResponse>
            .SuccessAsCreated(response, $"/api/v1/etkinliks/{newEtkinlik.Id}");
        }
    }
}
