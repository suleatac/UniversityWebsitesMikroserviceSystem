using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.BilgiFeatures.UpdateBilgi
{
    public class UpdateBilgiCommandHandler(
          IBilgiRepository bilgiRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<UpdateBilgiCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateBilgiCommand request, CancellationToken cancellationToken)
        {
            var bilgi = await bilgiRepository.GetByIdAsync(request.Id);
            if (bilgi == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            bilgi.Baslik = request.Baslik;
            bilgi.KisaAciklama = request.KisaAciklama;
            bilgi.IcerikMetni = request.IcerikMetni;
            bilgi.Link = request.Link;
            bilgi.ResimUrl = request.ResimUrl;
            bilgi.YayimTarihi = request.YayimTarihi;
            bilgi.BaslamaTarihi = request.BaslamaTarihi;
            bilgi.BitisTarihi = request.BitisTarihi;
            bilgi.SeoUrl = request.SeoUrl;
            bilgi.SeoTitle = request.SeoTitle;
            bilgi.SeoDescription = request.SeoDescription;
            bilgi.SiteId = request.SiteId;
            bilgi.DilId = request.DilId;
            bilgi.HedefId = request.HedefId;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new BilgiDeletedEvent(bilgi.SiteId, bilgi.DilId), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
