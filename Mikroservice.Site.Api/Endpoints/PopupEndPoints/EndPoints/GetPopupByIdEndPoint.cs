using MediatR;
using Mikroservice.Site.Application.DTOs.PopupDtos;
using Mikroservice.Site.Application.Features.PopupFeatures.GetPopupById;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.PopupEndPoints.EndPoints
{
    public static class GetPopupByIdEndPoint
    {
        public static RouteGroupBuilder GetPopupByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetPopupByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetPopupById")
            .MapToApiVersion(1.0)
            .Produces<PopupDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}