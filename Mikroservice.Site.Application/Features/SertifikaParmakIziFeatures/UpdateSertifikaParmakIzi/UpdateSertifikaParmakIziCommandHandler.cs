using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SertifikaParmakIziEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.UpdateSertifikaParmakIzi
{
    public class UpdateSertifikaParmakIziCommandHandler(
          ISertifikaParmakIziRepository sertifikaParmakIziRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<UpdateSertifikaParmakIziCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateSertifikaParmakIziCommand request, CancellationToken cancellationToken)
        {
            var sertifikaParmakIzi = await sertifikaParmakIziRepository.GetByIdAsync(request.Id);
            if (sertifikaParmakIzi == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            sertifikaParmakIzi.SertifikaParmakIziNumarasi = request.SertifikaParmakIziNumarasi;
            sertifikaParmakIzi.SertifikaYili = request.SertifikaYili;
            sertifikaParmakIzi.Aktif = request.Aktif;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new SertifikaParmakIziChangedEvent(), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
