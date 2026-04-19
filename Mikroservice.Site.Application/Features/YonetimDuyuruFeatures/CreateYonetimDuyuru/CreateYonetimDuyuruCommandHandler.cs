using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Features.YonetimDuyuruFeatures.CreateYonetimDuyuru
{
    public class CreateYonetimDuyuruCommandHandler
        (
          IYonetimDuyuruRepository yonetimDuyuruRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCacheService
        )
        : IRequestHandler<CreateYonetimDuyuruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateYonetimDuyuruCommand request, CancellationToken cancellationToken)
        {
            var newYonetimDuyuru = new YonetimDuyuru {
                Baslik = request.Baslik,
                Icerik = request.Icerik,
                EklenmeTarihi = request.EklenmeTarihi,
                Aktif = request.Aktif
            };
            await yonetimDuyuruRepository.AddAsync(newYonetimDuyuru);
            await unitOfWork.SaveChangesAsync();

            var cacheKey = "list:yonetimDuyurulari";
            await redisCacheService.RemoveAsync(cacheKey, cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
