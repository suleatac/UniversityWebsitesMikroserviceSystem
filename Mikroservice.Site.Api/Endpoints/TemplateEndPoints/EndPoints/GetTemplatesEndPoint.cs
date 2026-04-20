using MediatR;
using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.TemplateFeatures.GetTemplate;

namespace Mikroservice.Site.Api.Endpoints.TemplateEndPoints.EndPoints
{
    public static class GetTemplatesEndPoint
    {
        public static RouteGroupBuilder GetTemplatesEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) => {
                var result = await mediator.Send(new GetTemplateQuery());
                return result.ToGenericResult();
            })
            .WithName("GetTemplates")
            .MapToApiVersion(1.0)
            .Produces<List<Template>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
