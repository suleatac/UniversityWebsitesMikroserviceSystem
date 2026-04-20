using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.UpdateSiteOzellikleri;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.SiteOzellikleriEndPoints.EndPoints
{
    public static class UpdateSiteOzellikleriEndPoint
    {
        public static RouteGroupBuilder UpdateSiteOzellikleriEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (
                int id,
                [FromServices] IMediator mediator,
                [FromBody] UpdateSiteOzellikleriCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("UpdateSiteOzellikleri")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateSiteOzellikleriCommand>>();

            return group;
        }
    }
}
