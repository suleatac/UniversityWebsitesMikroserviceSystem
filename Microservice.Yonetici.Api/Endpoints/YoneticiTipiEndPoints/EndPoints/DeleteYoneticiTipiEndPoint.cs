using MediatR;
using Microservice.Yonetici.Application.Features.YoneticiTipiFeatures.DeleteYoneticiTipi;
using Microservice.Shared.Extentions;

namespace Microservice.Yonetici.Api.Endpoints.YoneticiTipiEndPoints.EndPoints
{
    public static class DeleteYoneticiTipiEndPoint
    {
        public static RouteGroupBuilder DeleteYoneticiTipiEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) => {
                var result = await mediator.Send(new DeleteYoneticiTipiCommand(id));
                return result.ToGenericResult();
            })
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("DeleteYoneticiTipi")
            .WithSummary("Delete Yonetici Tipi")
            .WithDescription("Delete Yonetici Tipi by Id");
            return group;
        }
    }
}
