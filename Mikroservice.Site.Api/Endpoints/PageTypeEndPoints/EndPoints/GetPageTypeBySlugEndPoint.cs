using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.PageTypeFeatures.GetPageTypeBySlug;

namespace Mikroservice.Site.Api.Endpoints.PageTypeEndPoints.EndPoints
{
    public static class GetPageTypeBySlugEndPoint
    {
        public static RouteGroupBuilder GetPageTypeBySlugEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/slug/{siteId:int}/{dilId:int}/{slug}", async (IMediator mediator, int siteId, int dilId, string slug) =>
                (await mediator.Send(new GetPageTypeBySlugQuery(siteId, dilId, slug))).ToGenericResult())
                .WithName("GetPageTypeBySlug")
                .MapToApiVersion(1.0)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}