using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.VideoEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.VideoFeatures.UpdateVideo
{
    public class UpdateVideoCommandHandler(
       IVideoRepository repository,
       IUnitOfWork unitOfWork,
       IPublishEndpoint publishEndpoint
   ) : IRequestHandler<UpdateVideoCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateVideoCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            entity.Baslik = request.Baslik;
            entity.KisaAciklama = request.KisaAciklama;
            entity.IcerikMetni = request.IcerikMetni;

            entity.Link = request.Link;
            entity.ResimUrl = request.ResimUrl;

            entity.YayimTarihi = request.YayimTarihi;

            entity.BaslamaTarihi = request.BaslamaTarihi;
            entity.BitisTarihi = request.BitisTarihi;

            entity.SeoUrl = request.SeoUrl;
            entity.SeoTitle = request.SeoTitle;
            entity.SeoDescription = request.SeoDescription;

            entity.VideoUrl = request.VideoUrl;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(
                new VideoChangedEvent(entity.SiteId, entity.DilId),
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
