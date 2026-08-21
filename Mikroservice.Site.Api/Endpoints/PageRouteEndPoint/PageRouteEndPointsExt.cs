using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.PageRouteEndPoint.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.HaberEndPoints
{
    public static class PageRouteEndPointsExt
    {
        public static void AddPageRouteGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/page-routes")
                .WithTags("PageRoute")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.GetBySlugEndpointGroupItem();
            group.RequireAuthorization("ClientCredential");
        }
    }
}
