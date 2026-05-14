using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.DeleteSikcaSorulanSoru
{
    public class DeleteSikcaSorulanSoruCommandHandler(
         ISikcaSorulanSoruRepository sikcaSorulanSoruRepository,
         IUnitOfWork unitOfWork,
         IRedisCacheService redisCache
     ) : IRequestHandler<DeleteSikcaSorulanSoruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteSikcaSorulanSoruCommand request, CancellationToken cancellationToken)
        {
            var sikcaSorulanSoru = await sikcaSorulanSoruRepository.GetByIdAsync(request.Id);

            if (sikcaSorulanSoru == null || sikcaSorulanSoru.IsDeleted)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            // 🔥 ALT SORULARI DE SİL (recursive soft delete)
            await SoftDeleteRecursive(sikcaSorulanSoru.Id);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache invalidation
            var key = $"sikcasorulansoru:list:{sikcaSorulanSoru.SiteId}:{sikcaSorulanSoru.DilId}";
            await redisCache.RemoveAsync(key, cancellationToken);

            return ServiceResult.Success();
        }

        private async Task SoftDeleteRecursive(int sikcaSorulanSoruId)
        {
            var children = await sikcaSorulanSoruRepository.Where(x => x.ParentId == sikcaSorulanSoruId).ToListAsync();

            foreach (var child in children)
            {
                await SoftDeleteRecursive(child.Id);
            }

            var entity = await sikcaSorulanSoruRepository.GetByIdAsync(sikcaSorulanSoruId);
            if (entity != null)
            {
                entity.IsDeleted = true;
            }
        }
    }
}
