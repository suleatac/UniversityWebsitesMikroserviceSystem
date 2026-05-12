using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Microservice.Site.Application.Features.YonetimDuyuruFeatures.DeleteYonetimDuyuru
{
    public class DeleteYonetimDuyuruCommandHandler(
          IYonetimDuyuruRepository yonetimDuyuruRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCacheService
        )
        : IRequestHandler<DeleteYonetimDuyuruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteYonetimDuyuruCommand request, CancellationToken cancellationToken)
        {
            var yonetimDuyuru = await yonetimDuyuruRepository.GetByIdAsync(request.Id);
            if (yonetimDuyuru == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            yonetimDuyuru.IsDeleted = true;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var cacheKey = "yonetimduyuru:*";
            await redisCacheService.RemoveByPatternAsync(cacheKey, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
