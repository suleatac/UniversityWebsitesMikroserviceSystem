using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.HaberEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.HaberFeatures.CreateHaber
{
    public class CreateHaberCommandHandler(
          IHaberRepository haberRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<CreateHaberCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateHaberCommand request, CancellationToken cancellationToken)
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
            await publishEndpoint.Publish(new HaberCreatedEvent(newHaber.SiteId, newHaber.DilId), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
