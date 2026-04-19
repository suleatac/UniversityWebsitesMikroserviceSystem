using MediatR;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.BirimFeatures.CreateBirim;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.BirimEndPoints.EndPoints
{
    public static class CreateBirimEndPoint
    {
        public static RouteGroupBuilder CreateBirimEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateBirimCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateBirim")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateBirimCommand>>();

            return group;
        }
    }
}
