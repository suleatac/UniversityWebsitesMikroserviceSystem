using MediatR;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.BilgiDtos;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.BilgiFeatures.GetPaginatedBilgi;

namespace Mikroservice.Site.Api.Endpoints.BilgiEndPoints.EndPoints
{
    public static class GetPaginatedBilgiEndPoint
    {
        public static RouteGroupBuilder GetPaginatedBilgiEndpointGroupItem(this RouteGroupBuilder group)
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
                var query = new GetPaginatedBilgiQuery(siteId, dilId, page, pageSize, search, orderBy, orderDir);
                var result = await mediator.Send(query);
                return result.ToGenericResult();
            })
            .WithName("GetPaginatedBilgi")
            .MapToApiVersion(1.0)
            .Produces<PaginatedResult<BilgiDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}