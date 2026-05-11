using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.DuyuruEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.DuyuruEndPoints
{
    public static class DuyuruEndPointsExt
    {
        public static void AddDuyuruGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/duyurular")
                .WithTags("Duyuru")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateDuyuruEndpointGroupItem();
            group.UpdateDuyuruEndpointGroupItem();
            group.DeleteDuyuruEndpointGroupItem();
            group.GetDuyurularEndpointGroupItem();
            group.GetDuyuruByIdEndpointGroupItem();
            group.GetPaginatedDuyuruEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
