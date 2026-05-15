using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.PopupFeatures.UpdatePopup
{
    public class UpdatePopupCommandHandler(
          IPopupRepository popupRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<UpdatePopupCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdatePopupCommand request, CancellationToken cancellationToken)
        {
            var popup = await popupRepository.GetByIdAsync(request.Id);
            if (popup == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            popup.Baslik = request.Baslik;
            popup.KisaAciklama = request.KisaAciklama;
            popup.IcerikMetni = request.IcerikMetni;
            popup.Link = request.Link;
            popup.ResimUrl = request.ResimUrl;
            popup.YayimTarihi = request.YayimTarihi;
            popup.BaslamaTarihi = request.BaslamaTarihi;
            popup.BitisTarihi = request.BitisTarihi;
            popup.SeoUrl = request.SeoUrl;
            popup.SeoTitle = request.SeoTitle;
            popup.SeoDescription = request.SeoDescription;
            popup.SiteId = request.SiteId;
            popup.TamEkranMi = request.TamEkranMi;
            popup.GosterimSuresiSaniye = request.GosterimSuresiSaniye;
            popup.CookieIleTekrarGosterme = request.CookieIleTekrarGosterme;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var listKey = $"popup:list:{popup.SiteId}";
            await redisCache.RemoveAsync(listKey, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
