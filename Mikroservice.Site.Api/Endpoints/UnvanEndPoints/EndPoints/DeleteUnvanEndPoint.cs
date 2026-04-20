using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.UnvanFeatures.DeleteUnvan;

namespace Mikroservice.Site.Api.Endpoints.UnvanEndPoints.EndPoints
{
    public static class DeleteUnvanEndPoint
    {
        public static RouteGroupBuilder DeleteUnvanEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteUnvanCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteUnvan")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
