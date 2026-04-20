using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.YoneticiSiteEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.YoneticiSiteEndPoints
{
    public static class YoneticiSiteEndPointsExt
    {
        public static void AddYoneticiSiteGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/yonetici-siteler")
                .WithTags("YoneticiSite")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateYoneticiSiteEndpointGroupItem();
            group.UpdateYoneticiSiteEndpointGroupItem();
            group.DeleteYoneticiSiteEndpointGroupItem();
            group.GetYoneticiSitesEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
