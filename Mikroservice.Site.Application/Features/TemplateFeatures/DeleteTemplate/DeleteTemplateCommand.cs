using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.DeleteTemplate
{
    public record DeleteTemplateCommand(int Id) : IRequestByServiceResult;
}
