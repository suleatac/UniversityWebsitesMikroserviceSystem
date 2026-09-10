using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.IcerikEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.IcerikEndPoints
{
    public static class IcerikEndPointsExt
    {
        public static void AddIcerikGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/icerikler")
                .WithTags("Icerik")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.IsSeoUrlAvailableEndpointGroupItem();
            group.RequireAuthorization("ClientCredential");
        }
    }
}
