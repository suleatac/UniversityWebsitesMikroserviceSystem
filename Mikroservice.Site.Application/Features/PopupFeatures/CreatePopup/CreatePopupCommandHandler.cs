using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.PopupFeatures.CreatePopup
{
    public class CreatePopupCommandHandler(
          IPopupRepository popupRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<CreatePopupCommand, ServiceResult<CreatePopupResponse>>
    {
        public async Task<ServiceResult<CreatePopupResponse>> Handle(CreatePopupCommand request, CancellationToken cancellationToken)
        {
            var newPopup = new Popup {
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
            await popupRepository.AddAsync(newPopup);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var key = $"popup:list:{newPopup.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            var response = new CreatePopupResponse(newPopup.Id);
            return ServiceResult<CreatePopupResponse>
            .SuccessAsCreated(response, $"/api/v1/popups/{newPopup.Id}");
        }
    }
}
