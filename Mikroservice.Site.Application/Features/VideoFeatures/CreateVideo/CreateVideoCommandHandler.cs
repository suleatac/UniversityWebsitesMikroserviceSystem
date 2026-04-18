using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.VideoEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.VideoFeatures.CreateVideo
{
    public class CreateVideoCommandHandler(
     IVideoRepository repository,
     IUnitOfWork unitOfWork,
     IPublishEndpoint publishEndpoint
 ) : IRequestHandler<CreateVideoCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateVideoCommand request, CancellationToken cancellationToken)
        {
            var entity = new Video
            {
                SiteId = request.SiteId,
                DilId = request.DilId,
                HedefId = request.HedefId,

                Baslik = request.Baslik,
                KisaAciklama = request.KisaAciklama,
                IcerikMetni = request.IcerikMetni,

                Link = request.Link,
                ResimUrl = request.ResimUrl,

                YayimTarihi = request.YayimTarihi,
                EklemeTarihi = request.EklemeTarihi,

                BaslamaTarihi = request.BaslamaTarihi,
                BitisTarihi = request.BitisTarihi,

                SeoUrl = request.SeoUrl,
                SeoTitle = request.SeoTitle,
                SeoDescription = request.SeoDescription,

                VideoUrl = request.VideoUrl,

                GosterimSayisi = 0,
                IsDeleted = false
            };

            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(
                new VideoChangedEvent(request.SiteId, request.DilId),
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
