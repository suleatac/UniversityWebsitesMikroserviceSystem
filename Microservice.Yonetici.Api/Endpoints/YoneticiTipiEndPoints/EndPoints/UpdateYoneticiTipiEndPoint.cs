using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Microservice.Yonetici.Application.Features.YoneticiTipiFeatures.UpdateYoneticiTipi;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Yonetici.Api.Endpoints.YoneticiTipiEndPoints.EndPoints
{
    public static class UpdateYoneticiTipiEndPoint
    {
        public static RouteGroupBuilder UpdateYoneticiTipiEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/", async ([FromServices] IMediator mediator, [FromBody] UpdateYoneticiTipiCommand command) => {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
             .WithName("UpdateYoneticiTipi")
             .MapToApiVersion(1.0)
             .AddEndpointFilter<ValidationFilter<UpdateYoneticiTipiCommand>>()
             .Produces<Guid>(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
