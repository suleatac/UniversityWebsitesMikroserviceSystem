using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Site.Application.Features.PageTypeFeatures.DeletePageType;

namespace Mikroservice.Site.Api.Endpoints.PageTypeEndPoints.EndPoints
{
    public static class DeletePageTypeEndPoint
    {
        public static RouteGroupBuilder DeletePageTypeEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                return (await mediator.Send(new DeletePageTypeCommand(id))).ToGenericResult();
            })
            .WithName("DeletePageType")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}