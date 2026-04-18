using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SitePersonelEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.UpdateSitePersonel
{
    public class UpdateSitePersonelCommandHandler(
       ISitePersonelRepository repository,
       IUnitOfWork unitOfWork,
       IPublishEndpoint publishEndpoint
   ) : IRequestHandler<UpdateSitePersonelCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateSitePersonelCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            entity.UnvanId = request.UnvanId;
            entity.PersonelTipId = request.PersonelTipId;

            entity.ResimUrl = request.ResimUrl;
            entity.IlgiAlanlari = request.IlgiAlanlari;

            entity.BlogAdress = request.BlogAdress;
            entity.TwitterAdress = request.TwitterAdress;
            entity.FacebookAdress = request.FacebookAdress;
            entity.InstagramAdress = request.InstagramAdress;
            entity.GoogleplusAdress = request.GoogleplusAdress;

            entity.Hakkinda = request.Hakkinda;
            entity.DeneyimVeCalismalari = request.DeneyimVeCalismalari;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(
                new SitePersonelChangedEvent(entity.SiteId),
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
