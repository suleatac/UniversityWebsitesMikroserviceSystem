using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Yonetici.Application.Contracts.IRepositories;
using Microservice.Yonetici.Domain.Entities;

namespace Microservice.Yonetici.Application.Features.YoneticiTipiFeatures.CreateYoneticiTipi
{
    public class CreateYoneticiTipiCommandHandler
        (
          IYoneticiTipiRepository yoneticiTipiRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCacheService
        )
        : IRequestHandler<CreateYoneticiTipiCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateYoneticiTipiCommand request, CancellationToken cancellationToken)
        {
            var newYoneticiTipi = new YoneticiTipi
            {
                TipAdi = request.TipAdi,
                Value = request.Value
            };
            await yoneticiTipiRepository.AddAsync( newYoneticiTipi );
            await unitOfWork.SaveChangesAsync();

            var cacheKey = "list:yoneticiTipleri";
            await redisCacheService.RemoveAsync(cacheKey, cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
