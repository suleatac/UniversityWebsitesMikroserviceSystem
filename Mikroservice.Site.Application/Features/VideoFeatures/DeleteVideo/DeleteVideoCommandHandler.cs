using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.VideoEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.VideoFeatures.DeleteVideo
{
    public class DeleteVideoCommandHandler(
        IVideoRepository repository,
        IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint
    ) : IRequestHandler<DeleteVideoCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            entity.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(
                new VideoChangedEvent(entity.SiteId, entity.DilId),
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
