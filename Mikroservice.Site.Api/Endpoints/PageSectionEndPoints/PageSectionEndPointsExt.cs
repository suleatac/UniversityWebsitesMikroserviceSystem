using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.PageSectionEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.PageSectionEndPoints
{
    public static class PageSectionEndPointsExt
    {
        public static void AddPageSectionGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/page-sections")
                .WithTags("PageSection")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.GetPageSectionsEndpointGroupItem();
            group.GetPageSectionByIdEndpointGroupItem();
            group.CreatePageSectionEndpointGroupItem();
            group.UpdatePageSectionEndpointGroupItem();
            group.DeletePageSectionEndpointGroupItem();
            group.ReorderPageSectionsEndpointGroupItem();
            group.CreatePageBlockEndpointGroupItem();
            group.UpdatePageBlockEndpointGroupItem();
            group.DeletePageBlockEndpointGroupItem();
            group.RequireAuthorization("ClientCredential");
        }
    }
}
