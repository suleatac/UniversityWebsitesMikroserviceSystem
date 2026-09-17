using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.GaleriResimDtos;
using Mikroservice.Site.Application.Features.GaleriResimFeatures.GetGaleriResimler;

namespace Mikroservice.Site.Api.Endpoints.GaleriResimEndPoints.EndPoints
{
    public static class GetGaleriResimlerEndPoint
    {
        public static RouteGroupBuilder GetGaleriResimlerEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                int dilId,
                IMediator mediator) =>
            {
                if (siteId <= 0 || dilId <= 0)
                    return Results.BadRequest("Geçersiz siteId veya dilId");

                var result = await mediator.Send(
                    new GetGaleriResimlerQuery(siteId, dilId)
                );

                return result.ToGenericResult();
            })
            .WithName("GetGaleriResimler")
            .MapToApiVersion(1.0)
            .Produces<List<GaleriResimDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
