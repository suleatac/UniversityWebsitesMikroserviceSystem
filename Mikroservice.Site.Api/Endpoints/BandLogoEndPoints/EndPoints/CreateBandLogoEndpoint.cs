using MediatR;
using Microservice.Shared.Filters;
using Microservice.Shared.Extentions;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.BandLogoFeatures.CreateBandLogo;

namespace Mikroservice.Site.Api.Endpoints.BandLogoEndPoints.EndPoints
{
    public static class CreateBandLogoEndpoint
    {
        public static RouteGroupBuilder CreateBandLogoEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateBandLogoCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateBandLogo")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateBandLogoCommand>>();

            return group;
        }
    }
}
