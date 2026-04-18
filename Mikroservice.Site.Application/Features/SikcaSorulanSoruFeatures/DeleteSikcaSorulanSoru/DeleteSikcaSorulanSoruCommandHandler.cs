using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.DeleteSikcaSorulanSoru
{
    public class DeleteSikcaSorulanSoruCommandHandler(
          ISikcaSorulanSoruRepository sikcaSorulanSoruRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<DeleteSikcaSorulanSoruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteSikcaSorulanSoruCommand request, CancellationToken cancellationToken)
        {
            var sikcaSorulanSoru = await sikcaSorulanSoruRepository.GetByIdAsync(request.Id);
            if (sikcaSorulanSoru == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            sikcaSorulanSoru.IsDeleted = true;
            sikcaSorulanSoruRepository.Update(sikcaSorulanSoru);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new SikcaSorulanSoruChangedEvent(sikcaSorulanSoru.SiteId, sikcaSorulanSoru.DilId), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
