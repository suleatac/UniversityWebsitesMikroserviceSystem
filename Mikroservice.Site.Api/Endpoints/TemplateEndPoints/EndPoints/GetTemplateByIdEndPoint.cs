
using MediatR;
using Mikroservice.Site.Application.DTOs.TemplateDtos;
using Mikroservice.Site.Application.Features.TemplateFeatures.GetTemplateById;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.TemplateEndPoints.EndPoints
{
    public static class GetTemplateByIdEndPoint
    {
        public static RouteGroupBuilder GetTemplateByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}", async (IMediator mediator, int id) => {
                var result = await mediator.Send(new GetTemplateByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetTemplateById")
            .MapToApiVersion(1.0)
            .Produces<TemplateDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
