using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.GetTemplate
{
    public record GetTemplateQuery : IRequestByServiceResult<List<Template>>;
}
