using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.HaberEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.HaberEndPoints
{
    public static class HaberEndPointsExt
    {
        public static void AddHaberGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/haberler")
                .WithTags("Haber")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateHaberEndpointGroupItem();
            group.UpdateHaberEndpointGroupItem();
            group.DeleteHaberEndpointGroupItem();
            group.GetHaberlerEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
