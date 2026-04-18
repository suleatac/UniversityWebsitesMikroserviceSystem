using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruEvents;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruKategoriEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.DeleteSikcaSorulanSoruKategori
{
    public class DeleteSikcaSorulanSoruKategoriCommandHandler(
    ISikcaSorulanSoruKategoriRepository kategoriRepository,
    ISikcaSorulanSoruRepository soruRepository,
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint
) : IRequestHandler<DeleteSikcaSorulanSoruKategoriCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteSikcaSorulanSoruKategoriCommand request, CancellationToken cancellationToken)
        {
            var kategori = await kategoriRepository.GetByIdAsync(request.Id);

            if (kategori == null || kategori.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            // 🔥 İçindeki soruları da sil
            var sorular = soruRepository
                .Where(x => x.KategoriId == kategori.Id && !x.IsDeleted)
                .ToList();

            foreach (var soru in sorular)
            {
                soru.IsDeleted = true;
                await publishEndpoint.Publish(new SikcaSorulanSoruChangedEvent(soru.SiteId, soru.DilId), cancellationToken); // (isteğe bağlı)
            }

            kategori.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache temizleme
            await publishEndpoint.Publish(new SikcaSorulanSoruKategoriChangedEvent(), cancellationToken);
         

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
