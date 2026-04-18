using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruKategoriEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.CreateSikcaSorulanSoruKategori
{
    public class CreateSikcaSorulanSoruKategoriCommandHandler(
         ISikcaSorulanSoruKategoriRepository kategoriRepository,
         IUnitOfWork unitOfWork,
         IPublishEndpoint publishEndpoint
     ) : IRequestHandler<CreateSikcaSorulanSoruKategoriCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateSikcaSorulanSoruKategoriCommand request, CancellationToken cancellationToken)
        {
            var entity = new SikcaSorulanSoruKategori
            {
                Ad = request.Ad,
                Sira = request.Sira,
                IsDeleted = false
            };

            await kategoriRepository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache invalidation event
            await publishEndpoint.Publish(
                new SikcaSorulanSoruKategoriChangedEvent(),
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
