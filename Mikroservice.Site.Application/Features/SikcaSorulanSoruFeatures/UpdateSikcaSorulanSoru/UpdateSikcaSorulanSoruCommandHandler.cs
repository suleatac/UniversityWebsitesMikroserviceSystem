using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.UpdateSikcaSorulanSoru
{
    public class UpdateSikcaSorulanSoruCommandHandler(
          ISikcaSorulanSoruRepository sikcaSorulanSoruRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<UpdateSikcaSorulanSoruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateSikcaSorulanSoruCommand request, CancellationToken cancellationToken)
        {
            var sikcaSorulanSoru = await sikcaSorulanSoruRepository.GetByIdAsync(request.Id);
            if (sikcaSorulanSoru == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }
            sikcaSorulanSoru.SiteId = request.SiteId;
            sikcaSorulanSoru.DilId = request.DilId;
            sikcaSorulanSoru.ParentId = request.ParentId;
            sikcaSorulanSoru.Soru = request.Soru;
            sikcaSorulanSoru.Cevap = request.Cevap;
            sikcaSorulanSoru.Sira = request.Sira;
            sikcaSorulanSoru.SeoUrl = request.SeoUrl;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache silme işlemi
            var key = $"sikcasorulansoru:list:{sikcaSorulanSoru.SiteId}:*";
            await redisCache.RemoveAsync(key, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
