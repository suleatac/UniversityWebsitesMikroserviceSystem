using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.YoneticiSiteFeatures.UpdateYoneticiSite;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.YoneticiSiteEndPoints.EndPoints
{
    public static class UpdateYoneticiSiteEndPoint
    {
        public static RouteGroupBuilder UpdateYoneticiSiteEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (
                int id,
                [FromServices] IMediator mediator,
                [FromBody] UpdateYoneticiSiteCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("UpdateYoneticiSite")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateYoneticiSiteCommand>>();

            return group;
        }
    }
}
