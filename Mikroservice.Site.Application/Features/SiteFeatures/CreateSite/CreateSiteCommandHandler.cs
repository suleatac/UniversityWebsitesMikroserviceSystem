using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SiteEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SiteFeatures.CreateSite
{
    public class CreateSiteCommandHandler(
     ISiteRepository siteRepository,
     IUnitOfWork unitOfWork,
     IPublishEndpoint publishEndpoint
 ) : IRequestHandler<CreateSiteCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateSiteCommand request, CancellationToken cancellationToken)
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

                SertifikaParmakIziId = request.SertifikaParmakIziId,
                TemplateId = request.TemplateId,

                IsDeleted = false
            };

            await siteRepository.AddAsync(site);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new SiteChangedEvent(), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
