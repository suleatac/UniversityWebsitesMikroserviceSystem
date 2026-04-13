using MediatR;
using Microservice.Shared.Filters;
using Microservice.Yonetici.Application.Features.YoneticiDuyuruFeatures.UpdateYoneticiDuyuru;
using Microservice.Shared.Extentions;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Yonetici.Api.Endpoints.YoneticiDuyuruEndPoints.EndPoints
{
    public static class UpdateYoneticiDuyuruEndPoint
    {
        public static RouteGroupBuilder UpdateYoneticiDuyuruEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/", async ([FromServices] IMediator mediator, [FromBody] UpdateYoneticiDuyuruCommand command) => {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
             .WithName("UpdateYoneticiDuyuru")
             .MapToApiVersion(1.0)
             .AddEndpointFilter<ValidationFilter<UpdateYoneticiDuyuruCommand>>()
             .Produces<Guid>(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
