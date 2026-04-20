using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.CreateSiteOzellikleri;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.SiteOzellikleriEndPoints.EndPoints
{
    public static class CreateSiteOzellikleriEndPoint
    {
        public static RouteGroupBuilder CreateSiteOzellikleriEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateSiteOzellikleriCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateSiteOzellikleri")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateSiteOzellikleriCommand>>();

            return group;
        }
    }
}
