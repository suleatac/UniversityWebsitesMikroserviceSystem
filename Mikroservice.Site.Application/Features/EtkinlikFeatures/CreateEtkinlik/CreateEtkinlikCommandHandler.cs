using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.EtkinlikEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.CreateEtkinlik
{
    public class CreateEtkinlikCommandHandler(
          IEtkinlikRepository etkinlikRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<CreateEtkinlikCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateEtkinlikCommand request, CancellationToken cancellationToken)
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

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new EtkinlikCreatedEvent(newEtkinlik.SiteId, newEtkinlik.DilId), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
