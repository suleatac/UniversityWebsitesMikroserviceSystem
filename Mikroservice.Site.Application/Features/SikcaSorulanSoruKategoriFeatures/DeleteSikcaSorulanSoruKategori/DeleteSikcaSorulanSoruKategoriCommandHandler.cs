using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruEvents;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruEvents.Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruEvents;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruKategoriEvents;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.DeleteSikcaSorulanSoruKategori
{
    public class DeleteSikcaSorulanSoruKategoriCommandHandler(
    ISikcaSorulanSoruKategoriRepository kategoriRepository,
    ISikcaSorulanSoruRepository soruRepository,
    IUnitOfWork unitOfWork,
    IRedisCacheService redisCache
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
            
                // 🔥 Cache silme işlemi
                var cachekey = $"sikcasorulansoru:list:{soru.SiteId}:*";
                await redisCache.RemoveAsync(cachekey, cancellationToken);


            }

            kategori.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache invalidation event
            var key = $"sikcaSorulanSoruKategori:list";
            await redisCache.RemoveAsync(key, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
