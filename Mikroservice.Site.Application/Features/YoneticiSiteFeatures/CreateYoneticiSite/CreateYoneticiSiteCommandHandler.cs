using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.YoneticiSiteEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.CreateYoneticiSite
{
    public class CreateYoneticiSiteCommandHandler(
     IYoneticiSiteRepository repository,
     IUnitOfWork unitOfWork,
     IPublishEndpoint publishEndpoint
 ) : IRequestHandler<CreateYoneticiSiteCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateYoneticiSiteCommand request, CancellationToken cancellationToken)
        {
            var exists = await repository.AnyWithKeycloakUserIdSiteIdYoneticiTipiIdAsync(request.KeycloakUserId,request.SiteId,request.YoneticiTipiId,
                cancellationToken);

            if (exists)
                return ServiceResult.Error("Bu kullanıcı zaten bu role sahip.",System.Net.HttpStatusCode.BadRequest);

            var entity = new YoneticiSite
            {
                KeycloakUserId = request.KeycloakUserId,
                SiteId = request.SiteId,
                YoneticiTipiId = request.YoneticiTipiId,
                IsDeleted = false
            };

            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(
                new YoneticiSiteChangedEvent(request.SiteId),
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
