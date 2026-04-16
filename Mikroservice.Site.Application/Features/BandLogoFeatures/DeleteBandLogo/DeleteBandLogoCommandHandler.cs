using MassTransit;
using MassTransit.Transports;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.DeleteBandLogo
{
    public class DeleteBandLogoCommandHandler(
          IBandLogoRepository bandLogoRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<DeleteBandLogoCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteBandLogoCommand request, CancellationToken cancellationToken)
        {
            var bandLogo = await bandLogoRepository.GetByIdAsync(request.Id);
            if (bandLogo == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            bandLogoRepository.Delete(bandLogo);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new BandLogoDeletedEvent(bandLogo.SiteId, bandLogo.DilId), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
