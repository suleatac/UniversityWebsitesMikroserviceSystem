using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.VideoFeatures.CreateVideo;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.VideoEndPoints.EndPoints
{
    public static class CreateVideoEndPoint
    {
        public static RouteGroupBuilder CreateVideoEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateVideoCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateVideo")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateVideoCommand>>();

            return group;
        }
    }
}
