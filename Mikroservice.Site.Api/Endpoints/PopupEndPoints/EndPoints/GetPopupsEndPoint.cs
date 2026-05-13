using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.PopupFeatures.GetPopup;

namespace Mikroservice.Site.Api.Endpoints.PopupEndPoints.EndPoints
{
    public static class GetPopupsEndPoint
    {
        public static RouteGroupBuilder GetPopupsEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new GetPopupQuery(siteId));
                return result.ToGenericResult();
            })
            .WithName("GetPopupBySiteId")
            .MapToApiVersion(1.0)
            .Produces<Popup>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
