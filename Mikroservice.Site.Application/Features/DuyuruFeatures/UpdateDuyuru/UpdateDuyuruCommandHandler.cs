using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.DuyuruEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.UpdateDuyuru
{
    public class UpdateDuyuruCommandHandler(
          IDuyuruRepository duyuruRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<UpdateDuyuruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateDuyuruCommand request, CancellationToken cancellationToken)
        {
            var duyuru = await duyuruRepository.GetByIdAsync(request.Id);
            if (duyuru == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            duyuru.Baslik = request.Baslik;
            duyuru.KisaAciklama = request.KisaAciklama;
            duyuru.IcerikMetni = request.IcerikMetni;
            duyuru.Link = request.Link;
            duyuru.ResimUrl = request.ResimUrl;
            duyuru.YayimTarihi = request.YayimTarihi;
            duyuru.BaslamaTarihi = request.BaslamaTarihi;
            duyuru.BitisTarihi = request.BitisTarihi;
            duyuru.SeoUrl = request.SeoUrl;
            duyuru.SeoTitle = request.SeoTitle;
            duyuru.SeoDescription = request.SeoDescription;
            duyuru.SiteId = request.SiteId;
            duyuru.DilId = request.DilId;
            duyuru.HedefId = request.HedefId;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new DuyuruUpdatedEvent(duyuru.SiteId, duyuru.DilId), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
