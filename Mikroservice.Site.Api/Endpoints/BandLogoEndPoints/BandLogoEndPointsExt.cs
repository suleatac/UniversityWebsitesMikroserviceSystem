using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.BandLogoEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.BandLogoEndPoints
{
    public static class BandLogoEndPointsExt
    {
        public static void AddBandLogoGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/bandlogos")
                .WithTags("BandLogo")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization(); // 🔥 policy burada

            group.MapToApiVersion(1.0);

            group.CreateBandLogoEndpointGroupItem();
            group.UpdateBandLogoEndpointGroupItem();
            group.DeleteBandLogoEndpointGroupItem();
            group.GetBandLogosEndpointGroupItem();

            group.RequireAuthorization("Password");
        }
    }
}
