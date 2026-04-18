using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.YoneticiSiteEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.UpdateYoneticiSite
{
    public class UpdateYoneticiSiteCommandHandler(
    IYoneticiSiteRepository repository,
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint
) : IRequestHandler<UpdateYoneticiSiteCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateYoneticiSiteCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            entity.YoneticiTipiId = request.YoneticiTipiId;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(
                new YoneticiSiteChangedEvent(entity.SiteId),
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
