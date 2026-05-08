using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SiteFeatures.UpdateSite
{
    public class UpdateSiteCommandHandler(
     ISiteRepository siteRepository,
     IUnitOfWork unitOfWork,
     IRedisCacheService redisCache
 ) : IRequestHandler<UpdateSiteCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateSiteCommand request, CancellationToken cancellationToken)
        {
            var site = await siteRepository.GetByIdAsync(request.Id);

            if (site == null || site.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            site.SiteAdi = request.SiteAdi;
            site.SiteAdiEng = request.SiteAdiEng;
            site.SiteUrl = request.SiteUrl;
            site.BirimId = request.BirimId;
            site.SiteAlanAdi = request.SiteAlanAdi;

            site.SiteEPosta = request.SiteEPosta;
            site.SiteEPostaSifre = request.SiteEPostaSifre;
            site.SiteEPostaHost = request.SiteEPostaHost;
            site.SiteEPostaPort = request.SiteEPostaPort;
            site.TemplateId = request.TemplateId;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Cache invalidation
            await redisCache.RemoveByPatternAsync(
                "site:*",
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
