using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruKategoriEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.UpdateSikcaSorulanSoruKategori
{
    public class UpdateSikcaSorulanSoruKategoriCommandHandler(
     ISikcaSorulanSoruKategoriRepository kategoriRepository,
     IUnitOfWork unitOfWork,
     IPublishEndpoint publishEndpoint
 ) : IRequestHandler<UpdateSikcaSorulanSoruKategoriCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateSikcaSorulanSoruKategoriCommand request, CancellationToken cancellationToken)
        {
            var kategori = await kategoriRepository.GetByIdAsync(request.Id);

            if (kategori == null || kategori.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            kategori.Ad = request.Ad;
            kategori.Sira = request.Sira;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache invalidation
            await publishEndpoint.Publish(new SikcaSorulanSoruKategoriChangedEvent(), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
