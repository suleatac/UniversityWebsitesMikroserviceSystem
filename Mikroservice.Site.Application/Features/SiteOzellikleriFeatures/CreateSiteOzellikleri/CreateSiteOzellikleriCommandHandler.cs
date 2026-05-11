using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SiteOzellikleriEvents;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.CreateSiteOzellikleri
{
    public class CreateSiteOzellikleriCommandHandler(
     ISiteOzellikleriRepository repository,
     IUnitOfWork unitOfWork,
     IRedisCacheService redisCache
 ) : IRequestHandler<CreateSiteOzellikleriCommand, ServiceResult<CreateSiteOzellikleriResponse>>
    {
        public async Task<ServiceResult<CreateSiteOzellikleriResponse>> Handle(CreateSiteOzellikleriCommand request, CancellationToken cancellationToken)
        {
            // 🔥 1-1 kontrol
            var exists = await repository.AnyBySiteIdAsync(request.SiteId, cancellationToken);

            if (exists)
                return ServiceResult<CreateSiteOzellikleriResponse>.Error("Bu site için özellik zaten tanımlı.",System.Net.HttpStatusCode.Conflict);

            var entity = new SiteOzellikleri
            {
                SiteId = request.SiteId,

                SiteAdress = request.SiteAdress,
                SiteAdressEng = request.SiteAdressEng,

                SiteBaslangicHakkimizda = request.SiteBaslangicHakkimizda,
                SiteBaslangicHakkimizdaEng = request.SiteBaslangicHakkimizdaEng,

                SiteTelNo = request.SiteTelNo,
                SiteFaxNo = request.SiteFaxNo,

                SiteFacebookAdress = request.SiteFacebookAdress,
                SiteTwitterAdress = request.SiteTwitterAdress,
                SiteInstagramAdress = request.SiteInstagramAdress,
                SiteYoutubeAdress = request.SiteYoutubeAdress,
                SiteLinkedinAdress = request.SiteLinkedinAdress,

                SiteHaritaAdress = request.SiteHaritaAdress,

                SiteBaslangicVideoLink = request.SiteBaslangicVideoLink,
                SiteBaslangicVideoResimAdress = request.SiteBaslangicVideoResimAdress,
                SiteVideoType = request.SiteVideoType,

                SiteWatsappAdress = request.SiteWatsappAdress,

                SiteHakkindaLink = request.SiteHakkindaLink,
                SiteHakkindaResim = request.SiteHakkindaResim,

                SiteFooterLogo = request.SiteFooterLogo,
                SiteTopbarLogo = request.SiteTopbarLogo
            };

            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);

         
            var key = $"siteozellikleri:{entity.SiteId}";
            await redisCache.RemoveAsync(key, cancellationToken);



            var response = new CreateSiteOzellikleriResponse(entity.Id);
            return ServiceResult<CreateSiteOzellikleriResponse>
            .SuccessAsCreated(response, $"/api/v1/site-ozellikleri/{entity.Id}");
        }
    }
}
