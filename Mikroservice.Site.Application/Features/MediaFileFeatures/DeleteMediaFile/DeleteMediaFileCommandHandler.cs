using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.MediaFileEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.MediaFileFeatures.DeleteMediaFile
{
    public class DeleteMediaFileCommandHandler(
          IMediaFileRepository mediaFileRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<DeleteMediaFileCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteMediaFileCommand request, CancellationToken cancellationToken)
        {
            var mediaFile = await mediaFileRepository.GetByIdAsync(request.Id);
            if (mediaFile == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            mediaFileRepository.Delete(mediaFile);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new MediaFileChangedEvent(mediaFile.SiteId, mediaFile.DilId), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
