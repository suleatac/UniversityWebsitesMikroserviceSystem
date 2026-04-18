using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SiteEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SiteFeatures.DeleteSite
{
    public class DeleteSiteCommandHandler(
     ISiteRepository siteRepository,
     IUnitOfWork unitOfWork,
     IPublishEndpoint publishEndpoint
 ) : IRequestHandler<DeleteSiteCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteSiteCommand request, CancellationToken cancellationToken)
        {
            var site = await siteRepository.GetByIdAsync(request.Id);

            if (site == null || site.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            site.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new SiteChangedEvent(), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
