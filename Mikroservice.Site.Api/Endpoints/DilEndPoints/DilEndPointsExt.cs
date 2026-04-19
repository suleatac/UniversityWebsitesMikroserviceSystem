using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.DilEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.DilEndPoints
{
    public static class DilEndPointsExt
    {
        public static void AddDilGroupsEndpointExt(
        this WebApplication app,
        ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/dils")
                .WithTags("Dil")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);
            group.GetDilsEndpointGroupItem();


            group.RequireAuthorization("Password");
        }
    }
}
