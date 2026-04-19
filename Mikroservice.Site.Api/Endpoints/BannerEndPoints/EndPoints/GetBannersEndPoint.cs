using MediatR;
using Mikroservice.Site.Application.Features.BannerFeatures.GetBanners;
using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.BannerEndPoints.EndPoints
{
    public static class GetBannersEndPoint
    {
        public static RouteGroupBuilder GetBannersEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                int dilId,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new GetBannersQuery(siteId, dilId));
                return result.ToGenericResult();
            })
            .WithName("GetBanners")
            .MapToApiVersion(1.0)
            .Produces<List<Banner>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
