using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BirimEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.BirimFeatures.CreateBirim
{
    public class CreateBirimCommandHandler(
          IBirimRepository birimRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<CreateBirimCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateBirimCommand request, CancellationToken cancellationToken)
        {
            var newBirim = new Birim {
                Ad = request.Ad,
                ParentId = request.ParentId,
                Sira = request.Sira,     
            };

            await birimRepository.AddAsync(newBirim);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // ✔ DOĞRU EVENT
            await publishEndpoint.Publish(
                new BirimCreatedEvent(),
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
