using MassTransit;
using MassTransit.Transports;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.CreateBandLogo
{
    public class CreateBandLogoCommandHandler(
          IBandLogoRepository bandLogoRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<CreateBandLogoCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateBandLogoCommand request, CancellationToken cancellationToken)
        {
            var newBandLogo = new BandLogo {
                Ad = request.Ad,
                ImgUrl = request.ImgUrl,
                Link = request.Link,
                SiteId = request.SiteId,
                DilId = request.DilId
            };
            await bandLogoRepository.AddAsync(newBandLogo);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new BandLogoDeletedEvent(request.SiteId, request.DilId), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
