using Asp.Versioning.Builder;

namespace Mikroservice.Ogrenci.Api.Endpoints.OgrenciEndPoints.OgrenciEndPoints
{
    public static class OgrenciEndpointExtentions
    {
        public static void AddOgrenciGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            var group = app.MapGroup("/api/v{version:apiVersion}/ogrencis").WithTags("Ogrencis");
            group.GetOgrencisGroupItemEndpoint();
            group.WithApiVersionSet(apiVersionSet);
            group.RequireAuthorization("Password");
        }
    }
}
