using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.PageBlockFeatures.UpdatePageBlock;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.PageSectionEndPoints.EndPoints
{
    public static class UpdatePageBlockEndPoint
    {
        public static RouteGroupBuilder UpdatePageBlockEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/blocks/{blockId:int}", async (
                int blockId,
                [FromServices] IMediator mediator,
                [FromBody] UpdatePageBlockCommand command) =>
            {
                if (blockId != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("UpdatePageBlock")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdatePageBlockCommand>>();

            return group;
        }
    }
}
