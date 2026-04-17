using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.HaberEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.HaberFeatures.DeleteHaber
{
    public class DeleteHaberCommandHandler(
          IHaberRepository haberRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<DeleteHaberCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteHaberCommand request, CancellationToken cancellationToken)
        {
            var haber = await haberRepository.GetByIdAsync(request.Id);
            if (haber == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            haber.IsDeleted = true;
            haberRepository.Update(haber);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new HaberDeletedEvent(haber.SiteId, haber.DilId), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
