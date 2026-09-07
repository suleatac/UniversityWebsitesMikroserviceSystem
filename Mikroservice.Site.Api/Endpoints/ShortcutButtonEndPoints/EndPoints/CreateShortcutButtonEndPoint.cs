using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.ShortcutButtonFeatures.CreateShortcutButton;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.ShortcutButtonEndPoints.EndPoints
{
    public static class CreateShortcutButtonEndPoint
    {
        public static RouteGroupBuilder CreateShortcutButtonEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateShortcutButtonCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateShortcutButton")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateShortcutButtonCommand>>();

            return group;
        }
    }
}
