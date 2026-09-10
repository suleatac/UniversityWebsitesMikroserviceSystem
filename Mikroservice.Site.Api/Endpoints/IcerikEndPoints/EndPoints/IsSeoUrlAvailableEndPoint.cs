using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.IcerikFeatures.IsSeoUrlAvailable;

namespace Mikroservice.Site.Api.Endpoints.IcerikEndPoints.EndPoints
{
    public static class IsSeoUrlAvailableEndPoint
    {
        public static RouteGroupBuilder IsSeoUrlAvailableEndpointGroupItem(
            this RouteGroupBuilder group)
        {
            group.MapGet(
                "/seo-available/{siteId:int}/{pageTypeId:int}/{seoUrl}",
                async (
                    IMediator mediator,
                    int siteId,
                    string seoUrl,
                    int pageTypeId,
                    int? excludeIcerikId) => {
                        var result = await mediator.Send(
                        new IsSeoUrlAvailableQuery(
                            siteId,
                            seoUrl,
                            pageTypeId,
                            excludeIcerikId));

                        return result.ToGenericResult();
                    })
                .WithName("IsSeoUrlAvailable")
                .MapToApiVersion(1.0)
                .Produces<bool>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

            return group;
        }
    }
}
