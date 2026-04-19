using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Microservice.Site.Application.Features.YonetimDuyuruFeatures.UpdateYonetimDuyuru
{
    public class UpdateYonetimDuyuruCommandHandler(
          IYonetimDuyuruRepository yonetimDuyuruRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCacheService
        )
        : IRequestHandler<UpdateYonetimDuyuruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateYonetimDuyuruCommand request, CancellationToken cancellationToken)
        {
            var yonetimDuyuru = await yonetimDuyuruRepository.GetByIdAsync(request.Id);
            if (yonetimDuyuru == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            yonetimDuyuru.Baslik = request.Baslik;
            yonetimDuyuru.Icerik = request.Icerik;
            yonetimDuyuru.EklenmeTarihi = request.EklenmeTarihi;
            yonetimDuyuru.Aktif = request.Aktif;
            await unitOfWork.SaveChangesAsync();

            var cacheKey = "list:yonetimDuyurulari";
            await redisCacheService.RemoveAsync(cacheKey, cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
