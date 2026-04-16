using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Microservice.Site.Application.Features.YoneticiDuyuruFeatures.UpdateYoneticiDuyuru
{
    public class UpdateYoneticiDuyuruCommandHandler(
          IYonetimDuyuruRepository yoneticiDuyuruRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCacheService
        )
        : IRequestHandler<UpdateYoneticiDuyuruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateYoneticiDuyuruCommand request, CancellationToken cancellationToken)
        {
            var yoneticiDuyuru = await yoneticiDuyuruRepository.GetByIdAsync(request.Id);
            if (yoneticiDuyuru == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            yoneticiDuyuru.Baslik = request.Baslik;
            yoneticiDuyuru.Icerik = request.Icerik;
            yoneticiDuyuru.EklenmeTarihi = request.EklenmeTarihi;
            yoneticiDuyuru.Aktif = request.Aktif;
            await unitOfWork.SaveChangesAsync();

            var cacheKey = "list:yoneticiDuyurulari";
            await redisCacheService.RemoveAsync(cacheKey, cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
