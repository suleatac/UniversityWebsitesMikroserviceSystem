using MediatR;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.PopupDtos;
using Mikroservice.Site.Application.Features.PopupFeatures.GetPaginatedPopup;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.PopupEndPoints.EndPoints
{
    public static class GetPaginatedPopupEndPoint
    {
        public static RouteGroupBuilder GetPaginatedPopupEndpointGroupItem(this RouteGroupBuilder group)
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
                var query = new GetPaginatedPopupQuery(siteId, dilId, page, pageSize, search, orderBy, orderDir);
                var result = await mediator.Send(query);
                return result.ToGenericResult();
            })
            .WithName("GetPaginatedPopup")
            .MapToApiVersion(1.0)
            .Produces<PaginatedResult<PopupDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}