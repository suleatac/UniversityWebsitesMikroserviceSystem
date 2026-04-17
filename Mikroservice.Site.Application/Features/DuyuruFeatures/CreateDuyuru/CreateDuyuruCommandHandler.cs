using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.DuyuruEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.CreateDuyuru
{
    public class CreateDuyuruCommandHandler(
          IDuyuruRepository duyuruRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<CreateDuyuruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateDuyuruCommand request, CancellationToken cancellationToken)
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

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new DuyuruCreatedEvent(newDuyuru.SiteId, newDuyuru.DilId), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
