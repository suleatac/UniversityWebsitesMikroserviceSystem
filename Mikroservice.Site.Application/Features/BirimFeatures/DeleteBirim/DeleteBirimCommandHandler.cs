using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BirimEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.BirimFeatures.DeleteBirim
{
    public class DeleteBirimCommandHandler(
          IBirimRepository birimRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<DeleteBirimCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteBirimCommand request, CancellationToken cancellationToken)
        {
            var birim = await birimRepository.GetByIdAsync(request.Id);
            if (birim == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            birim.IsDeleted = true;
            birimRepository.Update(birim);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new BirimDeletedEvent(), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
