using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Microservice.Site.Application.Features.YoneticiTipiFeatures.DeleteYoneticiTipi
{
    public class DeleteYoneticiTipiCommandHandler(
          IYoneticiTipiRepository yoneticiTipiRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCacheService
        )
        : IRequestHandler<DeleteYoneticiTipiCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteYoneticiTipiCommand request, CancellationToken cancellationToken)
        {
            var yoneticiTipi = await yoneticiTipiRepository.GetByIdAsync(request.Id);
            if (yoneticiTipi == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            yoneticiTipiRepository.Delete(yoneticiTipi);

            await unitOfWork.SaveChangesAsync();
            var cacheKey = "list:yoneticiTipleri";
            await redisCacheService.RemoveAsync(cacheKey, cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
