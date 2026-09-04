using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.PageTypeFeatures.GetPageTypeBySlug;

namespace Mikroservice.Site.Api.Endpoints.PageTypeEndPoints.EndPoints
{
    public static class GetPageTypeBySlugEndPoint
    {
        public static RouteGroupBuilder GetPageTypeBySlugEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/slug/{siteTemplateId:int}/{dilId:int}/{slug}", async (IMediator mediator, int siteTemplateId, int dilId, string slug) =>
                (await mediator.Send(new GetPageTypeBySlugQuery(siteTemplateId, dilId, slug))).ToGenericResult())
                .WithName("GetPageTypeBySlug")
                .MapToApiVersion(1.0)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}