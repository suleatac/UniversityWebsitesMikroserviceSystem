using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.VideoFeatures.UpdateVideo;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Mikroservice.Site.Api.Endpoints.VideoEndPoints.EndPoints
{
    public static class UpdateVideoEndPoint
    {
        public static RouteGroupBuilder UpdateVideoEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (
                int id,
                [FromServices] IMediator mediator,
                [FromBody] UpdateVideoCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("UpdateVideo")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateVideoCommand>>();

            return group;
        }
    }
}
