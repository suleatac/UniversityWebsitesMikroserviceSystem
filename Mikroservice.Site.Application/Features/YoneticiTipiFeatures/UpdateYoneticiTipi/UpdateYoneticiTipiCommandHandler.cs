using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Microservice.Site.Application.Features.YoneticiTipiFeatures.UpdateYoneticiTipi
{
    public class UpdateYoneticiTipiCommandHandler
        (
          IYoneticiTipiRepository yoneticiTipiRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCacheService
        ) 
        : IRequestHandler<UpdateYoneticiTipiCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateYoneticiTipiCommand request, CancellationToken cancellationToken)
        {


            var yoneticiTipi = await yoneticiTipiRepository.GetByIdAsync(request.Id);
            if (yoneticiTipi == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            yoneticiTipi.TipAdi = request.TipAdi;
            yoneticiTipi.Value = request.Value;

            await unitOfWork.SaveChangesAsync();

            var cacheKey = "list:yoneticiTipleri";
            await redisCacheService.RemoveAsync(cacheKey, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
