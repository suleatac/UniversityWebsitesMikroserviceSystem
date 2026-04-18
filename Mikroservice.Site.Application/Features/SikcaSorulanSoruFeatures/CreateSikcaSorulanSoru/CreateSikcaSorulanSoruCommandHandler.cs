using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.CreateSikcaSorulanSoru
{
    public class CreateSikcaSorulanSoruCommandHandler(
           ISikcaSorulanSoruRepository soruRepository,
           IUnitOfWork unitOfWork,
           IPublishEndpoint publishEndpoint
       ) : IRequestHandler<CreateSikcaSorulanSoruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateSikcaSorulanSoruCommand request, CancellationToken cancellationToken)
        {
            var entity = new SikcaSorulanSoru
            {
                SiteId = request.SiteId,
                DilId = request.DilId,
                KategoriId = request.KategoriId,

                Soru = request.Soru,
                Cevap = request.Cevap,

                Sira = request.Sira,
                SeoUrl = request.SeoUrl,

                IsDeleted = false
            };

            await soruRepository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache invalidation event
            await publishEndpoint.Publish(
                new SikcaSorulanSoruChangedEvent(request.SiteId, request.DilId),
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
