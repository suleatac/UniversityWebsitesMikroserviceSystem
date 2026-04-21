using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.MediaFileEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.MediaFileFeatures.UpdateMediaFile
{
    public class UpdateMediaFileCommandHandler(
          IMediaFileRepository mediaFileRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<UpdateMediaFileCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateMediaFileCommand request, CancellationToken cancellationToken)
        {
            var mediaFile = await mediaFileRepository.GetByIdAsync(request.Id);
            if (mediaFile == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            mediaFile.Path = request.Path;
            mediaFile.Url = request.Url;
            mediaFile.SiteId = request.SiteId;
            mediaFile.DilId = request.DilId;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new MediaFileChangedEvent(mediaFile.SiteId, mediaFile.DilId), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
