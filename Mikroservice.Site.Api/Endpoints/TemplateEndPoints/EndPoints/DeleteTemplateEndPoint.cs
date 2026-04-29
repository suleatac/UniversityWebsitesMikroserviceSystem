using MediatR;
using Mikroservice.Site.Application.Features.TemplateFeatures.DeleteTemplate;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.TemplateEndPoints.EndPoints
{
    public static class DeleteTemplateEndPoint
    {
        public static RouteGroupBuilder DeleteTemplateEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) => {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteTemplateCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteTemplate")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
