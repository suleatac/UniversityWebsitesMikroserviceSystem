using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.UpdateSiteOzellikleri
{
    public class UpdateSiteOzellikleriCommandHandler(
      ISiteOzellikleriRepository repository,
      IUnitOfWork unitOfWork,
      IRedisCacheService redisCache
  ) : IRequestHandler<UpdateSiteOzellikleriCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateSiteOzellikleriCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null)
                return ServiceResult.ErrorAsNotFound();

            entity.SiteAdress = request.SiteAdress;
            entity.SiteAdressEng = request.SiteAdressEng;

            entity.SiteBaslangicHakkimizda = request.SiteBaslangicHakkimizda;
            entity.SiteBaslangicHakkimizdaEng = request.SiteBaslangicHakkimizdaEng;

            entity.SiteTelNo = request.SiteTelNo;
            entity.SiteFaxNo = request.SiteFaxNo;

            entity.SiteFacebookAdress = request.SiteFacebookAdress;
            entity.SiteTwitterAdress = request.SiteTwitterAdress;
            entity.SiteInstagramAdress = request.SiteInstagramAdress;
            entity.SiteYoutubeAdress = request.SiteYoutubeAdress;
            entity.SiteLinkedinAdress = request.SiteLinkedinAdress;

            entity.SiteHaritaAdress = request.SiteHaritaAdress;

            entity.SiteBaslangicVideoLink = request.SiteBaslangicVideoLink;
            entity.SiteBaslangicVideoResimAdress = request.SiteBaslangicVideoResimAdress;
            entity.SiteVideoType = request.SiteVideoType;

            entity.SiteWatsappAdress = request.SiteWatsappAdress;

            entity.SiteHakkindaLink = request.SiteHakkindaLink;
            entity.SiteHakkindaResim = request.SiteHakkindaResim;

            entity.SiteFooterLogo = request.SiteFooterLogo;
            entity.SiteTopbarLogo = request.SiteTopbarLogo;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var key = $"siteozellikleri:{entity.SiteId}";
            await redisCache.RemoveAsync(key, cancellationToken);
            return ServiceResult.Success();
        }
    }
}
