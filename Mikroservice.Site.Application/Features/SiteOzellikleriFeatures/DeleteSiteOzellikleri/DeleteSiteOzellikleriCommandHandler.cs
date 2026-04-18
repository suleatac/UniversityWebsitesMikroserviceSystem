using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SiteEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.DeleteSiteOzellikleri
{
    public class DeleteSiteOzellikleriCommandHandler(
      ISiteOzellikleriRepository repository,
      IUnitOfWork unitOfWork,
      IPublishEndpoint publishEndpoint
  ) : IRequestHandler<DeleteSiteOzellikleriCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteSiteOzellikleriCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null)
                return ServiceResult.ErrorAsNotFound();

            repository.Delete(entity);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new SiteChangedEvent(), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
