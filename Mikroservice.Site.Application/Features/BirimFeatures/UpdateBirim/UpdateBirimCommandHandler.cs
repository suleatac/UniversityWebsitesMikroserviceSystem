using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BirimEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.BirimFeatures.UpdateBirim
{
    public class UpdateBirimCommandHandler(
          IBirimRepository birimRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<UpdateBirimCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateBirimCommand request, CancellationToken cancellationToken)
        {
            var birim = await birimRepository.GetByIdAsync(request.Id);
            if (birim == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            birim.Ad = request.Ad;
            birim.ParentId = request.ParentId;
            birim.Sira = request.Sira;

       
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new BirimUpdatedEvent(), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
