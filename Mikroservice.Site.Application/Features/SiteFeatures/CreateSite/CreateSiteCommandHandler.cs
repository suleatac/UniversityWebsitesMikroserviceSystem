using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SiteFeatures.CreateSite
{
    public class CreateSiteCommandHandler(
     ISiteRepository siteRepository,
     IUnitOfWork unitOfWork,
      IRedisCacheService redisCache
 ) : IRequestHandler<CreateSiteCommand, ServiceResult<CreateSiteResponse>>
    {
        public async Task<ServiceResult<CreateSiteResponse>> Handle(CreateSiteCommand request, CancellationToken cancellationToken)
        {
            var site = new Domain.Entities.Site
            {
                SiteAdi = request.SiteAdi,
                SiteAdiEng = request.SiteAdiEng,
                SiteUrl = request.SiteUrl,
                BirimId = request.BirimId,
                SiteAlanAdi = request.SiteAlanAdi,

                SiteEPosta = request.SiteEPosta,
                SiteEPostaSifre = request.SiteEPostaSifre,
                SiteEPostaHost = request.SiteEPostaHost,
                SiteEPostaPort = request.SiteEPostaPort,
                TemplateId = request.TemplateId,

                IsDeleted = false
            };

            await siteRepository.AddAsync(site);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Cache invalidation
            await redisCache.RemoveByPatternAsync(
                "site:*",
                cancellationToken);

            var response = new CreateSiteResponse(site.Id);
            return ServiceResult<CreateSiteResponse>
            .SuccessAsCreated(response, $"/api/v1/sites/{site.Id}");
        }
    }
}
