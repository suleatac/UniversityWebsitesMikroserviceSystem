using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.TemplateEvents;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.CreateTemplate
{
    public class CreateTemplateCommandHandler(
     ITemplateRepository templateRepository,
     IUnitOfWork unitOfWork,
     IPublishEndpoint publishEndpoint
 ) : IRequestHandler<CreateTemplateCommand, ServiceResult<CreateTemplateResponse>>
    {
        public async Task<ServiceResult<CreateTemplateResponse>> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
        {
            var template = new Domain.Entities.Template {
                TemplateAdi = request.TemplateAdi,
                TemplateTuru = request.TemplateTuru,
                FolderName = request.FolderName,
                LayoutPath = request.LayoutPath
            };

            await templateRepository.AddAsync(template);
            await unitOfWork.SaveChangesAsync(cancellationToken);


            await publishEndpoint.Publish(new TemplateChangedEvent(), cancellationToken);

            var response = new CreateTemplateResponse(template.Id);
            return ServiceResult<CreateTemplateResponse>
            .SuccessAsCreated(response, $"/api/v1/templates/{template.Id}");

        }
    }
}
