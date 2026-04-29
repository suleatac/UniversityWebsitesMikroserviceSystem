using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.TemplateEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.DeleteTemplate
{
    public class DeleteTemplateCommandHandler(
          ITemplateRepository templateRepository,
          IUnitOfWork unitOfWork,
          IPublishEndpoint publishEndpoint
        )
        : IRequestHandler<DeleteTemplateCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = await templateRepository.GetByIdAsync(request.Id);
            if (template == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            templateRepository.Delete(template);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemini yapabilsin diye bu event eklendi.
            await publishEndpoint.Publish(new TemplateChangedEvent(), cancellationToken);


            return ServiceResult.SuccessAsNoContent();
        }
    }
}
