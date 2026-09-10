using MediatR;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.BandLogoDtos;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.BandLogoFeatures.GetPaginatedBandLogo;

namespace Mikroservice.Site.Api.Endpoints.BandLogoEndPoints.EndPoints
{
    public static class GetPaginatedBandLogoEndPoint
    {
        public static RouteGroupBuilder GetPaginatedBandLogoEndpointGroupItem(this RouteGroupBuilder group)
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
                var query = new GetPaginatedBandLogoQuery(siteId, dilId, page, pageSize, search, orderBy, orderDir);
                var result = await mediator.Send(query);
                return result.ToGenericResult();
            })
            .WithName("GetPaginatedBandLogo")
            .MapToApiVersion(1.0)
            .Produces<PaginatedResult<BandLogoDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
