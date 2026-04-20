using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.PopupFeatures.CreatePopup;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.PopupEndPoints.EndPoints
{
    public static class CreatePopupEndPoint
    {
        public static RouteGroupBuilder CreatePopupEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreatePopupCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreatePopup")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreatePopupCommand>>();

            return group;
        }
    }
}
