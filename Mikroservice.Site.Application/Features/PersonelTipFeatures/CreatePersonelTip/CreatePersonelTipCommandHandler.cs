using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.PersonelTipEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.CreatePersonelTip
{
    public class CreatePersonelTipCommandHandler(
          IPersonelTipRepository personelTipRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<CreatePersonelTipCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreatePersonelTipCommand request, CancellationToken cancellationToken)
        {
            var newPersonelTip = new PersonelTip {
                Ad = request.Ad
            };
            await personelTipRepository.AddAsync(newPersonelTip);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new PersonelTipChangedEvent(), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
