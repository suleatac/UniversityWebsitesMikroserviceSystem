using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.PageTypeFeatures.GetHomePageType;

namespace Mikroservice.Site.Api.Endpoints.PageTypeEndPoints.EndPoints
{
    public static class GetHomePageTypeEndPoint
    {
        public static RouteGroupBuilder GetHomePageTypeEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/home/{siteTemplateId:int}/{dilId:int}", async (IMediator mediator, int siteTemplateId, int dilId) =>
                (await mediator.Send(new GetHomePageTypeQuery(siteTemplateId, dilId))).ToGenericResult())
                .WithName("GetHomePageType")
                .MapToApiVersion(1.0)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}