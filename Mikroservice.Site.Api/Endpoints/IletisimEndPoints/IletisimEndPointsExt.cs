using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.IletisimEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.IletisimEndPoints
{
    public static class IletisimEndPointsExt
    {
        public static void AddIletisimGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/iletisim-mesajlari")
                .WithTags("Iletisim")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.GonderIletisimMesajiEndpointGroupItem();
            group.RequireAuthorization("ClientCredential");
        }
    }
}
