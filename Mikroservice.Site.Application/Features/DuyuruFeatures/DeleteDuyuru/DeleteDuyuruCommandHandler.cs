using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.DuyuruEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.DeleteDuyuru
{
    public class DeleteDuyuruCommandHandler(
          IDuyuruRepository duyuruRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<DeleteDuyuruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteDuyuruCommand request, CancellationToken cancellationToken)
        {
            var duyuru = await duyuruRepository.GetByIdAsync(request.Id);
            if (duyuru == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            duyuru.IsDeleted = true;
            duyuruRepository.Update(duyuru);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new DuyuruDeletedEvent(duyuru.SiteId, duyuru.DilId), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
