using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BilgiEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.BilgiFeatures.DeleteBilgi
{
    public class DeleteBilgiCommandHandler(
          IBilgiRepository bilgiRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<DeleteBilgiCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteBilgiCommand request, CancellationToken cancellationToken)
        {
            var bilgi = await bilgiRepository.GetByIdAsync(request.Id);
            if (bilgi == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            bilgi.IsDeleted = true;
            bilgiRepository.Update(bilgi);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new BilgiDeletedEvent(bilgi.SiteId, bilgi.DilId), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
