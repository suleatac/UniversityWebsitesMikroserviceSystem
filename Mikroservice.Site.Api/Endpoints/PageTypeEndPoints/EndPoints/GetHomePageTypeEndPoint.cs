using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.PageTypeFeatures.GetHomePageType;

namespace Mikroservice.Site.Api.Endpoints.PageTypeEndPoints.EndPoints
{
    public static class GetHomePageTypeEndPoint
    {
        public static RouteGroupBuilder GetHomePageTypeEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/home/{siteId:int}/{dilId:int}", async (IMediator mediator, int siteId, int dilId) =>
                (await mediator.Send(new GetHomePageTypeQuery(siteId, dilId))).ToGenericResult())
                .WithName("GetHomePageType")
                .MapToApiVersion(1.0)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}