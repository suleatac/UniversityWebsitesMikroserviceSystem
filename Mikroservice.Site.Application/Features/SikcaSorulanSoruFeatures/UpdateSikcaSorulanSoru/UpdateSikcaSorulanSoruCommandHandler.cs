using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.UpdateSikcaSorulanSoru
{
    public class UpdateSikcaSorulanSoruCommandHandler(
          ISikcaSorulanSoruRepository sikcaSorulanSoruRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<UpdateSikcaSorulanSoruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateSikcaSorulanSoruCommand request, CancellationToken cancellationToken)
        {
            var sikcaSorulanSoru = await sikcaSorulanSoruRepository.GetByIdAsync(request.Id);
            if (sikcaSorulanSoru == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }
            sikcaSorulanSoru.SiteId = request.SiteId;
            sikcaSorulanSoru.DilId = request.DilId;
            sikcaSorulanSoru.KategoriId = request.KategoriId;
            sikcaSorulanSoru.Soru = request.Soru;
            sikcaSorulanSoru.Cevap = request.Cevap;
            sikcaSorulanSoru.Sira = request.Sira;
            sikcaSorulanSoru.SeoUrl = request.SeoUrl;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new SikcaSorulanSoruChangedEvent(sikcaSorulanSoru.SiteId, sikcaSorulanSoru.DilId), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
