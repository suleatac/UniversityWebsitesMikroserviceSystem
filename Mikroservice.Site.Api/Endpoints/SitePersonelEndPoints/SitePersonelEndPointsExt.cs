using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.SitePersonelEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.SitePersonelEndPoints
{
    public static class SitePersonelEndPointsExt
    {
        public static void AddSitePersonelGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/site-personeller")
                .WithTags("SitePersonel")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateSitePersonelEndpointGroupItem();
            group.UpdateSitePersonelEndpointGroupItem();
            group.DeleteSitePersonelEndpointGroupItem();
            group.GetSitePersonellerEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
