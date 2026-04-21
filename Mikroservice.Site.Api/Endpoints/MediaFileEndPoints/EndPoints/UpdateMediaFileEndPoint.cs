using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.MediaFileFeatures.UpdateMediaFile;

namespace Mikroservice.Site.Api.Endpoints.MediaFileEndPoints.EndPoints
{
    public static class UpdateMediaFileEndPoint
    {
        public static RouteGroupBuilder UpdateMediaFileEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (
                int id,
                [FromServices] IMediator mediator,
                [FromBody] UpdateMediaFileCommand command) => {
                    if (id != command.Id)
                        return Results.BadRequest("Id uyuşmuyor");

                    var result = await mediator.Send(command);
                    return result.ToGenericResult();
                })
            .WithName("UpdateMediaFile")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateMediaFileCommand>>();

            return group;
        }
    }
}
