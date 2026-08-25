using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.PageTypeEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.PageTypeEndPoints
{
    public static class PageTypeEndPointsExt
    {
        public static void AddPageTypeGroupsEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            var group = app.MapGroup("/api/v{version:apiVersion}/page-types")
                .WithTags("PageType")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization("ClientCredential");

            group.MapToApiVersion(1.0);
            group.GetPageTypesEndpointGroupItem();
            group.GetPageTypeByIdEndpointGroupItem();
            group.CreatePageTypeEndpointGroupItem();
            group.GetPageTypeBySlugEndpointGroupItem();
            group.GetHomePageTypeEndpointGroupItem();
            group.UpdatePageTypeEndpointGroupItem();
            group.DeletePageTypeEndpointGroupItem();
        }
    }
}