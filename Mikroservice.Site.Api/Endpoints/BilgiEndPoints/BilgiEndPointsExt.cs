using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.BilgiEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.BilgiEndPoints
{
    public static class BilgiEndPointsExt
    {
        public static void AddBilgiGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/bilgis")
                .WithTags("Bilgi")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateBilgiEndpointGroupItem();
            group.UpdateBilgiEndpointGroupItem();
            group.DeleteBilgiEndpointGroupItem();
            group.GetBilgisEndpointGroupItem();

            group.RequireAuthorization("Password");
        }
    }
}
