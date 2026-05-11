using MediatR;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.VideoDtos;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.VideoFeatures.GetPaginatedVideo;

namespace Mikroservice.Site.Api.Endpoints.VideoEndPoints.EndPoints
{
    public static class GetPaginatedVideoEndPoint
    {
        public static RouteGroupBuilder GetPaginatedVideoEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/paginated", async (
                int siteId,
                int dilId,
                int page,
                int pageSize,
                string? search,
                string? orderBy,
                string? orderDir,
                IMediator mediator) =>
            {
                var query = new GetPaginatedVideoQuery(siteId, dilId, page, pageSize, search, orderBy, orderDir);
                var result = await mediator.Send(query);
                return result.ToGenericResult();
            })
            .WithName("GetPaginatedVideo")
            .MapToApiVersion(1.0)
            .Produces<PaginatedResult<VideoDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}