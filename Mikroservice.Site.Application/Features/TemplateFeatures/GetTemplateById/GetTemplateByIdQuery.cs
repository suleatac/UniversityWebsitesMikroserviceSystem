using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.TemplateDtos;

namespace Mikroservice.Site.Application.Features.TemplateFeatures.GetTemplateById
{
    public record GetTemplateByIdQuery(int Id) : IRequestByServiceResult<TemplateDetailDto>;

}
