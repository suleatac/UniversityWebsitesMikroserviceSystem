using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.UnvanEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.UpdateUnvan
{
    public class UpdateUnvanCommandHandler(
     IUnvanRepository repository,
     IUnitOfWork unitOfWork,
     IPublishEndpoint publishEndpoint
 ) : IRequestHandler<UpdateUnvanCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateUnvanCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null)
                return ServiceResult.ErrorAsNotFound();

            entity.TipId = request.TipId;
            entity.Ad = request.Ad;
            entity.KisaAd = request.KisaAd;
            entity.Sira = request.Sira;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new UnvanChangedEvent(), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
