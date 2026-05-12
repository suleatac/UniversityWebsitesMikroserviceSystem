using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.DeleteSikcaSorulanSoru
{
    public class DeleteSikcaSorulanSoruCommandHandler(
          ISikcaSorulanSoruRepository sikcaSorulanSoruRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<DeleteSikcaSorulanSoruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteSikcaSorulanSoruCommand request, CancellationToken cancellationToken)
        {
            var sikcaSorulanSoru = await sikcaSorulanSoruRepository.GetByIdAsync(request.Id);
            if (sikcaSorulanSoru == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            sikcaSorulanSoru.IsDeleted = true;
            sikcaSorulanSoruRepository.Update(sikcaSorulanSoru);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache silme işlemi
            var key = $"sikcasorulansoru:list:{sikcaSorulanSoru.SiteId}:*";
            await redisCache.RemoveAsync(key, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
