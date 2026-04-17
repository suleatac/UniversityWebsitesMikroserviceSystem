using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.EtkinlikEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.DeleteEtkinlik
{
    public class DeleteEtkinlikCommandHandler(
          IEtkinlikRepository etkinlikRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<DeleteEtkinlikCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteEtkinlikCommand request, CancellationToken cancellationToken)
        {
            var etkinlik = await etkinlikRepository.GetByIdAsync(request.Id);
            if (etkinlik == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            etkinlik.IsDeleted = true;
            etkinlikRepository.Update(etkinlik);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new EtkinlikDeletedEvent(etkinlik.SiteId, etkinlik.DilId), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
