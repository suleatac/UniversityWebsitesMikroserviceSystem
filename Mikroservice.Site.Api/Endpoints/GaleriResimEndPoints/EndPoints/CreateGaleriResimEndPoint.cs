using Microservice.Shared.Filters;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.GaleriResimFeatures.CreateGaleriResim;

namespace Mikroservice.Site.Api.Endpoints.GaleriResimEndPoints.EndPoints
{
    public static class CreateGaleriResimEndPoint
    {
        public static RouteGroupBuilder CreateGaleriResimEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateGaleriResimCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateGaleriResim")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateGaleriResimCommand>>();

            return group;
        }
    }
}
