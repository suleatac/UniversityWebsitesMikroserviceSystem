using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.UpdateYoneticiSite
{
    public class UpdateYoneticiSiteCommandHandler(
    IYoneticiSiteRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateYoneticiSiteCommandHandler> logger,
    IRedisCacheService redisCache
) : 
        IRequestHandler<UpdateYoneticiSiteCommand, ServiceResult<UpdateYoneticiSiteResponse>>
    {
        public async Task<ServiceResult<UpdateYoneticiSiteResponse>> Handle(UpdateYoneticiSiteCommand request, CancellationToken cancellationToken)
        {
            // ✔ DB'den TEK kayıt çek
            var entity = await repository.GetByIdAsync(request.Id);


            if (entity is null)
            {
                logger.LogWarning("Site bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<UpdateYoneticiSiteResponse>.Error("Site bulunamadı", HttpStatusCode.NotFound);
            }

            entity.YoneticiTipiId = request.YoneticiTipiId;
            entity.SiteId = request.SiteId;
            entity.KeycloakUserId = request.KeycloakUserId;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var key = "yoneticiSite:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);



              return ServiceResult<UpdateYoneticiSiteResponse>.SuccessAsOK(new UpdateYoneticiSiteResponse(entity.Id));

  
        }

      
    }
}
