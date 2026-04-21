using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.MediaFileEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.MediaFileFeatures.CreateMediaFile
{
    public class CreateMediaFileCommandHandler(
          IMediaFileRepository mediaFileRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<CreateMediaFileCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateMediaFileCommand request, CancellationToken cancellationToken)
        {
            var newMediaFile = new MediaFile {
                Path = request.Path,
                Url = request.Url,
                SiteId = request.SiteId,
                DilId = request.DilId

            };
            await mediaFileRepository.AddAsync(newMediaFile);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new MediaFileChangedEvent(newMediaFile.SiteId, newMediaFile.DilId), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
