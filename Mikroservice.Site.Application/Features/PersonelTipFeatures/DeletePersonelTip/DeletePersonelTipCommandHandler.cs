using MassTransit;
using MassTransit.Transports;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BandLogoEvents;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.PersonelTipEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Features.BandLogoFeatures.DeleteBandLogo;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.DeletePersonelTip
{
    public class DeletePersonelTipCommandHandler(
          IPersonelTipRepository personelTipRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<DeletePersonelTipCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeletePersonelTipCommand request, CancellationToken cancellationToken)
        {
            var personelTip = await personelTipRepository.GetByIdAsync(request.Id);
            if (personelTip == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            personelTipRepository.Delete(personelTip);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new PersonelTipChangedEvent(), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
