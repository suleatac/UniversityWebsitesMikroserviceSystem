using MassTransit;
using MassTransit.Transports;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Features.BandLogoFeatures.CreateBandLogo;
using Mikroservice.Site.Domain.Entities;
using System.Reflection;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

namespace Mikroservice.Site.Application.Features.BannerFeatures.CreateBanner
{
    public class CreateBannerCommandHandler(
          IBannerRepository bannerRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<CreateBannerCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateBannerCommand request, CancellationToken cancellationToken)
        {
            var newBanner = new Banner {
                Baslik = request.Baslik,
                KisaAciklama = request.KisaAciklama,
                IcerikMetni = request.IcerikMetni,
                Link = request.Link,
                ResimUrl = request.ResimUrl,
                Sira = request.Sira,
                YayimTarihi = request.YayimTarihi,
                BaslamaTarihi = request.BaslamaTarihi,
                BitisTarihi = request.BitisTarihi,
                SeoUrl = request.SeoUrl,
                SeoTitle = request.SeoTitle,
                SeoDescription = request.SeoDescription,
                SiteId = request.SiteId,
                DilId = request.DilId,
                HedefId=request.HedefId,

                EklemeTarihi = DateTime.Now,
                GosterimSayisi = 0,
                IsDeleted = false

            };
            await bannerRepository.AddAsync(newBanner);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new BannerDeletedEvent(newBanner.SiteId, newBanner.DilId), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
