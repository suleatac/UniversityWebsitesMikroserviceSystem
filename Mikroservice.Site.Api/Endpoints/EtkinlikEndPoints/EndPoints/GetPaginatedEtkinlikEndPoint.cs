using MediatR;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.EtkinlikDtos;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.EtkinlikFeatures.GetPaginatedEtkinlik;

namespace Mikroservice.Site.Api.Endpoints.EtkinlikEndPoints.EndPoints
{
    public static class GetPaginatedEtkinlikEndPoint
    {
        public static RouteGroupBuilder GetPaginatedEtkinlikEndpointGroupItem(this RouteGroupBuilder group)
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
                var query = new GetPaginatedEtkinlikQuery(siteId, dilId, page, pageSize, search, orderBy, orderDir);
                var result = await mediator.Send(query);
                return result.ToGenericResult();
            })
            .WithName("GetPaginatedEtkinlik")
            .MapToApiVersion(1.0)
            .Produces<PaginatedResult<EtkinlikDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}