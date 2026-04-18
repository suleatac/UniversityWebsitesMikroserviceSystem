using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SertifikaParmakIziEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.DeleteSertifikaParmakIzi
{
    public class DeleteSertifikaParmakIziCommandHandler(
          ISertifikaParmakIziRepository sertifikaParmakIziRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<DeleteSertifikaParmakIziCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteSertifikaParmakIziCommand request, CancellationToken cancellationToken)
        {
            var sertifikaParmakIzi = await sertifikaParmakIziRepository.GetByIdAsync(request.Id);
            if (sertifikaParmakIzi == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            sertifikaParmakIziRepository.Delete(sertifikaParmakIzi);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new SertifikaParmakIziChangedEvent(), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
