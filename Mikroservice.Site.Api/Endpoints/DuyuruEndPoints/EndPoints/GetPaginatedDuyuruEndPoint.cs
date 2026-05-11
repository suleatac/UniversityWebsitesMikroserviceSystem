using MediatR;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.DuyuruDtos;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.DuyuruFeatures.GetPaginatedDuyuru;

namespace Mikroservice.Site.Api.Endpoints.DuyuruEndPoints.EndPoints
{
    public static class GetPaginatedDuyuruEndPoint
    {
        public static RouteGroupBuilder GetPaginatedDuyuruEndpointGroupItem(this RouteGroupBuilder group)
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
                var query = new GetPaginatedDuyuruQuery(siteId, dilId, page, pageSize, search, orderBy, orderDir);
                var result = await mediator.Send(query);
                return result.ToGenericResult();
            })
            .WithName("GetPaginatedDuyuru")
            .MapToApiVersion(1.0)
            .Produces<PaginatedResult<DuyuruDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}