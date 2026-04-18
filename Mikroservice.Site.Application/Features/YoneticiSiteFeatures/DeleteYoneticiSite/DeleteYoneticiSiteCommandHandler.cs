using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.YoneticiSiteEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.DeleteYoneticiSite
{
    public class DeleteYoneticiSiteCommandHandler(
    IYoneticiSiteRepository repository,
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint
) : IRequestHandler<DeleteYoneticiSiteCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteYoneticiSiteCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            entity.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(
                new YoneticiSiteChangedEvent(entity.SiteId),
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
