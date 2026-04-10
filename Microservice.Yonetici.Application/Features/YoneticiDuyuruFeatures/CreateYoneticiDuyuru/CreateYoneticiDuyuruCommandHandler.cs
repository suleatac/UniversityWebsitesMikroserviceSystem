using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Yonetici.Application.Contracts.IRepositories;
using Microservice.Yonetici.Application.Features.YoneticiTipiFeatures.CreateYoneticiTipi;
using Microservice.Yonetici.Domain.Entities;

namespace Microservice.Yonetici.Application.Features.YoneticiDuyuruFeatures.CreateYoneticiDuyuru
{
    public class CreateYoneticiDuyuruCommandHandler
        (
          IYoneticiDuyuruRepository yoneticiDuyuruRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCacheService
        )
        : IRequestHandler<CreateYoneticiDuyuruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateYoneticiDuyuruCommand request, CancellationToken cancellationToken)
        {
            var newYoneticiDuyuru = new YoneticiDuyuru {
                Baslik = request.Baslik,
                Icerik = request.Icerik,
                EklenmeTarihi = request.EklenmeTarihi,
                Aktif = request.Aktif
            };
            await yoneticiDuyuruRepository.AddAsync(newYoneticiDuyuru);
            await unitOfWork.SaveChangesAsync();

            var cacheKey = "list:yoneticiDuyurulari";
            await redisCacheService.RemoveAsync(cacheKey, cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
