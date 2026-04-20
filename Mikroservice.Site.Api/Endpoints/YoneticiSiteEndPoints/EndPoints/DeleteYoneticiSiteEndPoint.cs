using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.YoneticiSiteFeatures.DeleteYoneticiSite;

namespace Mikroservice.Site.Api.Endpoints.YoneticiSiteEndPoints.EndPoints
{
    public static class DeleteYoneticiSiteEndPoint
    {
        public static RouteGroupBuilder DeleteYoneticiSiteEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteYoneticiSiteCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteYoneticiSite")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
