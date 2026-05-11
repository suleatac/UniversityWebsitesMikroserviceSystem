using MediatR;
using Mikroservice.Site.Application.DTOs.YoneticiSiteDtos;
using Mikroservice.Site.Application.Features.YoneticiSiteFeatures.GetYoneticiSiteById;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.YoneticiSiteEndPoints.EndPoints
{
    public static class GetYoneticiSiteByIdEndPoint
    {
        public static RouteGroupBuilder GetYoneticiSiteByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetYoneticiSiteByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetYoneticiSiteById")
            .MapToApiVersion(1.0)
            .Produces<YoneticiSiteDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}