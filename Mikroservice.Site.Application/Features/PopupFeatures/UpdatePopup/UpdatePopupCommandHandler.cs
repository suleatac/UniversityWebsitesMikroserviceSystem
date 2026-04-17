using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.PopupEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.PopupFeatures.UpdatePopup
{
    public class UpdatePopupCommandHandler(
          IPopupRepository popupRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
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
            popup.DilId = request.DilId;
            popup.HedefId = request.HedefId;
            popup.TamEkranMi = request.TamEkranMi;
            popup.GosterimSuresiSaniye = request.GosterimSuresiSaniye;
            popup.CookieIleTekrarGosterme = request.CookieIleTekrarGosterme;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new PopupChangedEvent(popup.SiteId, popup.DilId), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
