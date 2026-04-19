using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.MenuFeatures.CreateMenu;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.MenuEndPoints.EndPoints
{
    public static class CreateMenuEndPoint
    {
        public static RouteGroupBuilder CreateMenuEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateMenuCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateMenu")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateMenuCommand>>();

            return group;
        }
    }
}
