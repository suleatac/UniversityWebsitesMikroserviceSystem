using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BandLogoEvents;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.PersonelTipEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Features.BandLogoFeatures.UpdateBandLogo;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.UpdatePersonelTip
{
    public class UpdatePersonelTipCommandHandler(
          IPersonelTipRepository personelTipRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<UpdatePersonelTipCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdatePersonelTipCommand request, CancellationToken cancellationToken)
        {
            var personelTip = await personelTipRepository.GetByIdAsync(request.Id);
            if (personelTip == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            personelTip.Ad = request.Ad;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new PersonelTipChangedEvent(), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
