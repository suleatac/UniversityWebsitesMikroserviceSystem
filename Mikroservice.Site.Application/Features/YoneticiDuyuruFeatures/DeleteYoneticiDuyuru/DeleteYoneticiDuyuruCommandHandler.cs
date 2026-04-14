using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Microservice.Site.Application.Features.YoneticiDuyuruFeatures.DeleteYoneticiDuyuru
{
    public class DeleteYoneticiDuyuruCommandHandler(
          IYoneticiDuyuruRepository yoneticiDuyuruRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCacheService
        )
        : IRequestHandler<DeleteYoneticiDuyuruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteYoneticiDuyuruCommand request, CancellationToken cancellationToken)
        {
            var yoneticiDuyuru = await yoneticiDuyuruRepository.GetByIdAsync(request.Id);
            if (yoneticiDuyuru == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            yoneticiDuyuruRepository.Delete(yoneticiDuyuru);

            await unitOfWork.SaveChangesAsync();
            var cacheKey = "list:yoneticiDuyurulari";
            await redisCacheService.RemoveAsync(cacheKey, cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
