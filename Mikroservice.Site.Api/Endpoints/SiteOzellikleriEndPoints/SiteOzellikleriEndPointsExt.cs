using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.SiteOzellikleriEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.SiteOzellikleriEndPoints
{
    public static class SiteOzellikleriEndPointsExt
    {
        public static void AddSiteOzellikleriGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/site-ozellikleri")
                .WithTags("SiteOzellikleri")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateSiteOzellikleriEndpointGroupItem();
            group.UpdateSiteOzellikleriEndpointGroupItem();
            group.GetSiteOzellikleriEndpointGroupItem();

            group.RequireAuthorization("Password");
            // Genelde ekleme:
            // group.DeleteSiteOzellikleriEndpointGroupItem();
        }
    }
}
