using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SitePersonelEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.DeleteSitePersonel
{
    public class DeleteSitePersonelCommandHandler(
      ISitePersonelRepository repository,
      IUnitOfWork unitOfWork,
      IPublishEndpoint publishEndpoint
  ) : IRequestHandler<DeleteSitePersonelCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteSitePersonelCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            entity.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(
                new SitePersonelChangedEvent(entity.SiteId),
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
