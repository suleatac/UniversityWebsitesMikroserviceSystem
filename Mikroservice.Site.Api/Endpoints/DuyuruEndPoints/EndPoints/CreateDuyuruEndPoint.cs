using MediatR;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.DuyuruFeatures.CreateDuyuru;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.DuyuruEndPoints.EndPoints
{
    public static class CreateDuyuruEndPoint
    {
        public static RouteGroupBuilder CreateDuyuruEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateDuyuruCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateDuyuru")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateDuyuruCommand>>();

            return group;
        }
    }
}
