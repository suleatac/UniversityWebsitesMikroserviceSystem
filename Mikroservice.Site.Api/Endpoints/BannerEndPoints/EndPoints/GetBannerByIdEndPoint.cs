using MediatR;
using Mikroservice.Site.Application.DTOs.BannerDtos;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.BannerFeatures.GetBannerById;

namespace Mikroservice.Site.Api.Endpoints.BannerEndPoints.EndPoints
{
    public static class GetBannerByIdEndPoint
    {
        public static RouteGroupBuilder GetBannerByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetBannerByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetBannerById")
            .MapToApiVersion(1.0)
            .Produces<BannerDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}