using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.EtkinlikFeatures.CreateEtkinlik;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Mikroservice.Site.Api.Endpoints.EtkinlikEndPoints.EndPoints
{
    public static class CreateEtkinlikEndPoint
    {
        public static RouteGroupBuilder CreateEtkinlikEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateEtkinlikCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateEtkinlik")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateEtkinlikCommand>>();

            return group;
        }
    }
}
