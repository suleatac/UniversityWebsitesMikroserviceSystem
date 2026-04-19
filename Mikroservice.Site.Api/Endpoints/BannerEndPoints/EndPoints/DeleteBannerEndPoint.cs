
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.BannerFeatures.DeleteBanner;

namespace Mikroservice.Site.Api.Endpoints.BannerEndPoints.EndPoints
{
    public static class DeleteBannerEndPoint
    {
        public static RouteGroupBuilder DeleteBannerEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteBannerCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteBanner")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
