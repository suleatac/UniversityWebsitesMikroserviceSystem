using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.TemplateDtos;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.GetTemplate
{
    public record GetTemplateQuery : IRequestByServiceResult<List<TemplateListDto>>;
}
