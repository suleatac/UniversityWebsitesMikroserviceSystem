using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.BilgiFeatures.CreateBilgi
{
    public class CreateBilgiCommandHandler(
          IBilgiRepository bilgiRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<CreateBilgiCommand, ServiceResult<CreateBilgiResponse>>
    {
        public async Task<ServiceResult<CreateBilgiResponse>> Handle(CreateBilgiCommand request, CancellationToken cancellationToken)
        {
            var newBilgi = new Bilgi {
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
            await bilgiRepository.AddAsync(newBilgi);
            await unitOfWork.SaveChangesAsync(cancellationToken);


            //Cache temizleme işlemi.
            var cacheKey = $"bilgis:list:{newBilgi.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);


            var response = new CreateBilgiResponse(newBilgi.Id);
            return ServiceResult<CreateBilgiResponse>
            .SuccessAsCreated(response, $"/api/v1/bilgiler/{newBilgi.Id}");
        }
    }
}
