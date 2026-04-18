using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.UnvanEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.DeleteUnvan
{
    public class DeleteUnvanCommandHandler(
        IUnvanRepository repository,
        ISitePersonelRepository sitePersonelRepository,
        IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint
    ) : IRequestHandler<DeleteUnvanCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteUnvanCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null)
                return ServiceResult.ErrorAsNotFound();

            // 🔥 kullanım kontrolü
            var isUsed = await sitePersonelRepository.AnyByUnvanIdAsync(request.Id, cancellationToken);

            if (isUsed)
                return ServiceResult.Error("Bu ünvan kullanılıyor, silinemez.",System.Net.HttpStatusCode.Conflict);

            repository.Delete(entity);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await publishEndpoint.Publish(new UnvanChangedEvent(), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
