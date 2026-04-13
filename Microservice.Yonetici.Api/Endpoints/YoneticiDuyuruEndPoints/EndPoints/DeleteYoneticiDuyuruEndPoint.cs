using MediatR;
using Microservice.Yonetici.Application.Features.YoneticiDuyuruFeatures.DeleteYoneticiDuyuru;
using Microservice.Shared.Extentions;

namespace Microservice.Yonetici.Api.Endpoints.YoneticiDuyuruEndPoints.EndPoints
{
    public static class DeleteYoneticiDuyuruEndPoint
    {
        public static RouteGroupBuilder DeleteYoneticiDuyuruEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) => {
                var result = await mediator.Send(new DeleteYoneticiDuyuruCommand(id));
                return result.ToGenericResult();
            })
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("DeleteYoneticiDuyuru")
            .WithSummary("Delete Yonetici Duyuru")
            .WithDescription("Delete Yonetici Duyuru by Id");
            return group;
        }
    }
}
