using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.UnvanEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.CreateUnvan
{
    public class CreateUnvanCommandHandler(
     IUnvanRepository repository,
     IUnitOfWork unitOfWork,
     IPublishEndpoint publishEndpoint
 ) : IRequestHandler<CreateUnvanCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateUnvanCommand request, CancellationToken cancellationToken)
        {
            var entity = new Unvan
            {
                TipId = request.TipId,
                Ad = request.Ad,
                KisaAd = request.KisaAd,
                Sira = request.Sira
            };

            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new UnvanChangedEvent(), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
