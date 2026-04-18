using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SiteOzellikleriEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.CreateSiteOzellikleri
{
    public class CreateSiteOzellikleriCommandHandler(
     ISiteOzellikleriRepository repository,
     IUnitOfWork unitOfWork,
     IPublishEndpoint publishEndpoint
 ) : IRequestHandler<CreateSiteOzellikleriCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateSiteOzellikleriCommand request, CancellationToken cancellationToken)
        {
            // 🔥 1-1 kontrol
            var exists = await repository.AnyBySiteIdAsync(request.SiteId, cancellationToken);

            if (exists)
                return ServiceResult.Error("Bu site için özellik zaten tanımlı.",System.Net.HttpStatusCode.Conflict);

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

            await publishEndpoint.Publish(new SiteOzellikleriChangedEvent(entity.SiteId), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
