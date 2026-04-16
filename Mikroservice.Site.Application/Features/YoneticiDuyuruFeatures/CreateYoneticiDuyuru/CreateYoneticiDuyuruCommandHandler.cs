using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Features.YoneticiDuyuruFeatures.CreateYoneticiDuyuru
{
    public class CreateYoneticiDuyuruCommandHandler
        (
          IYonetimDuyuruRepository yoneticiDuyuruRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCacheService
        )
        : IRequestHandler<CreateYoneticiDuyuruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateYoneticiDuyuruCommand request, CancellationToken cancellationToken)
        {
            var newYoneticiDuyuru = new YonetimDuyuru {
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
