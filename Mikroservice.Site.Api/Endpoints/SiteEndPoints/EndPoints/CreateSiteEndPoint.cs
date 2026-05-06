using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.SiteFeatures.CreateSite;

namespace Mikroservice.Site.Api.Endpoints.SiteEndPoints.EndPoints
{
    public static class CreateSiteEndPoint
    {
        public static RouteGroupBuilder CreateSiteEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
               [FromServices] IMediator mediator,
               [FromBody] CreateSiteCommand command) => {
                   var result = await mediator.Send(command);
                   return result.ToGenericResult();
               })
           .WithName("CreateSite")
           .MapToApiVersion(1.0)
           .Produces(StatusCodes.Status204NoContent)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status500InternalServerError)
           .AddEndpointFilter<ValidationFilter<CreateSiteCommand>>();

            return group;
        }
    }
}
