using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SertifikaParmakIziEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.CreateSertifikaParmakIzi
{
    public class CreateSertifikaParmakIziCommandHandler(
          ISertifikaParmakIziRepository sertifikaParmakIziRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<CreateSertifikaParmakIziCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateSertifikaParmakIziCommand request, CancellationToken cancellationToken)
        {
            var newSertifikaParmakIzi = new SertifikaParmakIzi              
            {
                SertifikaParmakIziNumarasi = request.SertifikaParmakIziNumarasi,
                SertifikaYili = request.SertifikaYili,
                Aktif = request.Aktif
            };
            await sertifikaParmakIziRepository.AddAsync(newSertifikaParmakIzi);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new SertifikaParmakIziChangedEvent(), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
