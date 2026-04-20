using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.YoneticiSiteFeatures.CreateYoneticiSite;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.YoneticiSiteEndPoints.EndPoints
{
    public static class CreateYoneticiSiteEndPoint
    {
        public static RouteGroupBuilder CreateYoneticiSiteEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateYoneticiSiteCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateYoneticiSite")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateYoneticiSiteCommand>>();

            return group;
        }
    }
}
