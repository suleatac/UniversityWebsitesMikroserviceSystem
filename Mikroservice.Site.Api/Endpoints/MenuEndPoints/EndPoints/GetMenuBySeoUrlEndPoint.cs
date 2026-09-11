using MediatR;
using Mikroservice.Site.Application.DTOs.MenuDtos;
using Mikroservice.Site.Application.Features.MenuFeatures.GetMenuBySeoUrl;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.MenuEndPoints.EndPoints
{
    public static class GetMenuBySeoUrlEndPoint
    {
        public static RouteGroupBuilder GetMenuBySeoUrlEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/seo/{siteId:int}/{dilId:int}/{seoUrl}", async (IMediator mediator, int siteId, int dilId, string seoUrl) =>
                (await mediator.Send(new GetMenuBySeoUrlQuery(siteId, dilId, seoUrl))).ToGenericResult())
                .WithName("GetMenuBySeoUrl")
                .MapToApiVersion(1.0)
                .Produces<MenuDetailDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}
