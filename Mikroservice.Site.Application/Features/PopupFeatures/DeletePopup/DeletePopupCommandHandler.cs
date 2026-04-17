using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.PopupEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.PopupFeatures.DeletePopup
{
    public class DeletePopupCommandHandler(
          IPopupRepository popupRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<DeletePopupCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeletePopupCommand request, CancellationToken cancellationToken)
        {
            var popup = await popupRepository.GetByIdAsync(request.Id);
            if (popup == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            popup.IsDeleted = true;
            popupRepository.Update(popup);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new PopupChangedEvent(), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
