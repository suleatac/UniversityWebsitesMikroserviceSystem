using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.HaberEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.HaberFeatures.UpdateHaber
{
    public class UpdateHaberCommandHandler(
          IHaberRepository haberRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<UpdateHaberCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateHaberCommand request, CancellationToken cancellationToken)
        {
            var haber = await haberRepository.GetByIdAsync(request.Id);
            if (haber == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            haber.Baslik = request.Baslik;
            haber.KisaAciklama = request.KisaAciklama;
            haber.IcerikMetni = request.IcerikMetni;
            haber.Link = request.Link;
            haber.ResimUrl = request.ResimUrl;
            haber.YayimTarihi = request.YayimTarihi;
            haber.BaslamaTarihi = request.BaslamaTarihi;
            haber.BitisTarihi = request.BitisTarihi;
            haber.SeoUrl = request.SeoUrl;
            haber.SeoTitle = request.SeoTitle;
            haber.SeoDescription = request.SeoDescription;
            haber.SiteId = request.SiteId;
            haber.DilId = request.DilId;
            haber.HedefId = request.HedefId;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new HaberUpdatedEvent(haber.SiteId, haber.DilId), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
