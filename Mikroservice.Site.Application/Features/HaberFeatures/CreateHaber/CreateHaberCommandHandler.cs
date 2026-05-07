using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.HaberFeatures.CreateHaber
{
    public class CreateHaberCommandHandler(
     IHaberRepository haberRepository,
     IUnitOfWork unitOfWork,
      IRedisCacheService redisCache
 ) : IRequestHandler<CreateHaberCommand, ServiceResult<CreateHaberResponse>>
    {
        public async Task<ServiceResult<CreateHaberResponse>> Handle(CreateHaberCommand request, CancellationToken cancellationToken)
        {
            var newHaber = new Haber {
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
            await haberRepository.AddAsync(newHaber);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            var cacheKey = $"haber:list:{newHaber.SiteId}:{newHaber.DilId}:*";
            await redisCache.RemoveByPatternAsync(
                cacheKey,
                cancellationToken);


            var response = new CreateHaberResponse(newHaber.Id);
            return ServiceResult<CreateHaberResponse>
            .SuccessAsCreated(response, $"/api/v1/habers/{newHaber.Id}");
        }
    }
}
