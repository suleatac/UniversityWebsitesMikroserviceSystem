using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Microservice.Site.Application.Features.YoneticiTipiFeatures.CreateYoneticiTipi;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Site.Api.Endpoints.YoneticiTipiEndPoints.EndPoints
{
    public static class CreateYoneticiTipiEndPoint
    {
        public static RouteGroupBuilder CreateYoneticiTipiEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async ([FromServices] IMediator mediator, [FromBody] CreateYoneticiTipiCommand command) => {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
             .WithName("CreateYoneticiTipi")
             .MapToApiVersion(1.0)
             .Produces<Guid>(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError)
             .AddEndpointFilter<ValidationFilter<CreateYoneticiTipiCommand>>();
            return group;
        }
    }
}
