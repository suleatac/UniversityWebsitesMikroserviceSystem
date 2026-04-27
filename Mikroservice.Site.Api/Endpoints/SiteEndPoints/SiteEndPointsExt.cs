using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.SiteEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.SiteEndPoints
{
    public static class SiteEndPointsExt
    {
        public static void AddSiteGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/sites")
                .WithTags("Site")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateSiteEndpointGroupItem();
            group.UpdateSiteEndpointGroupItem();
            group.DeleteSiteEndpointGroupItem();
            group.GetSitesEndpointGroupItem();
            group.GetSiteByIdEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
