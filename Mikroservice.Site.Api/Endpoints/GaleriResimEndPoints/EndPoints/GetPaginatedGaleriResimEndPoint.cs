using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.GaleriResimDtos;
using Mikroservice.Site.Application.Features.GaleriResimFeatures.GetPaginatedGaleriResim;

namespace Mikroservice.Site.Api.Endpoints.GaleriResimEndPoints.EndPoints
{
    public static class GetPaginatedGaleriResimEndPoint
    {
        public static RouteGroupBuilder GetPaginatedGaleriResimEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/paginated", async (
                int siteId,
                int dilId,
                int page,
                int pageSize,
                string? search,
                string? kategori,
                string? orderBy,
                string? orderDir,
                IMediator mediator) =>
            {
                var query = new GetPaginatedGaleriResimQuery(siteId, dilId, page, pageSize, search, kategori, orderBy, orderDir);
                var result = await mediator.Send(query);
                return result.ToGenericResult();
            })
            .WithName("GetPaginatedGaleriResim")
            .MapToApiVersion(1.0)
            .Produces<PaginatedResult<GaleriResimDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
