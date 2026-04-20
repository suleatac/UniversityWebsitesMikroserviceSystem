using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Site.Application.Features.YonetimDuyuruFeatures.DeleteYonetimDuyuru;

namespace Microservice.Site.Api.Endpoints.YonetimDuyuruEndPoints.EndPoints
{
    public static class DeleteYonetimDuyuruEndPoint
    {
        public static RouteGroupBuilder DeleteYonetimDuyuruEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) => {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteYonetimDuyuruCommand(id));
                return result.ToGenericResult();
            })
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("DeleteYonetimDuyuru")
            .WithSummary("Delete Yonetim Duyuru")
            .WithDescription("Delete Yonetim Duyuru by Id");

            return group;
        }
    }
}
