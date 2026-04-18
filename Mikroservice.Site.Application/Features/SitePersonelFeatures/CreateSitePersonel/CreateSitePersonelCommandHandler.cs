using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SitePersonelEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.CreateSitePersonel
{
    public class CreateSitePersonelCommandHandler(
       ISitePersonelRepository repository,
       IUnitOfWork unitOfWork,
       IPublishEndpoint publishEndpoint
   ) : IRequestHandler<CreateSitePersonelCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateSitePersonelCommand request, CancellationToken cancellationToken)
        {
            var entity = new SitePersonel
            {
                SiteId = request.SiteId,
                PersonelId = request.PersonelId,
                UnvanId = request.UnvanId,
                PersonelTipId = request.PersonelTipId,

                ResimUrl = request.ResimUrl,
                IlgiAlanlari = request.IlgiAlanlari,
                BlogAdress = request.BlogAdress,
                TwitterAdress = request.TwitterAdress,
                FacebookAdress = request.FacebookAdress,
                InstagramAdress = request.InstagramAdress,
                GoogleplusAdress = request.GoogleplusAdress,

                Hakkinda = request.Hakkinda,
                DeneyimVeCalismalari = request.DeneyimVeCalismalari,

                IsDeleted = false
            };

            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(
                new SitePersonelChangedEvent(request.SiteId),
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
