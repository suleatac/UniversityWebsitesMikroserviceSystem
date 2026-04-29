using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.BirimEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.BirimEndPoints
{
    public static class BirimEndPointsExt
    {
        public static void AddBirimGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/birims")
                .WithTags("Birim")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateBirimEndpointGroupItem();
            group.UpdateBirimEndpointGroupItem();
            group.DeleteBirimEndpointGroupItem();
            group.GetBirimsEndpointGroupItem();
            group.GetBirimByIdEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
