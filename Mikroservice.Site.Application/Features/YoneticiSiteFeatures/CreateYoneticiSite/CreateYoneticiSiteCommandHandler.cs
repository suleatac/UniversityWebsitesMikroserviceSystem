using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.YoneticiSiteFeatures.CreateYoneticiSite
{
    public class CreateYoneticiSiteCommandHandler(
     IYoneticiSiteRepository repository,
     IUnitOfWork unitOfWork,
     IRedisCacheService redisCache
  ) : IRequestHandler<CreateYoneticiSiteCommand, ServiceResult<YoneticiSiteResponse>>
    {
        public async Task<ServiceResult<YoneticiSiteResponse>> Handle(CreateYoneticiSiteCommand request, CancellationToken cancellationToken)
        {
            var exists = await repository.AnyWithKeycloakUserIdSiteIdYoneticiTipiIdAsync(request.KeycloakUserId, request.SiteId, cancellationToken);

            if (exists)
                return ServiceResult<YoneticiSiteResponse>.Error("Bu kullanıcı zaten bu role sahip.", System.Net.HttpStatusCode.BadRequest);

            var entity = new YoneticiSite
            {
                KeycloakUserId = request.KeycloakUserId,
                SiteId = request.SiteId,
                IsDeleted = false
            };

            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);


            var key = "yoneticiSite:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            var response = new YoneticiSiteResponse(entity.Id);
            return ServiceResult<YoneticiSiteResponse>
            .SuccessAsCreated(response, $"/api/v1/yonetici-sites/{entity.Id}");
        }
    }
}
