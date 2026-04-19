
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.BilgiFeatures.DeleteBilgi;

namespace Mikroservice.Site.Api.Endpoints.BilgiEndPoints.EndPoints
{
    public static class DeleteBilgiEndPoint
    {
        public static RouteGroupBuilder DeleteBilgiEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteBilgiCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteBilgi")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
