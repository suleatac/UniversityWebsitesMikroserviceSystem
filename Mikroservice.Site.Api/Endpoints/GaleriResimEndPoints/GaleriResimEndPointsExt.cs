using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.GaleriResimEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.GaleriResimEndPoints
{
    public static class GaleriResimEndPointsExt
    {
        public static void AddGaleriResimGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/galeri-resimler")
                .WithTags("GaleriResim")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateGaleriResimEndpointGroupItem();
            group.UpdateGaleriResimEndpointGroupItem();
            group.DeleteGaleriResimEndpointGroupItem();
            group.GetGaleriResimlerEndpointGroupItem();
            group.GetGaleriResimByIdEndpointGroupItem();
            group.GetPaginatedGaleriResimEndpointGroupItem();
            group.GetGaleriResimBySeoUrlEndpointGroupItem();
            group.RequireAuthorization("ClientCredential");
        }
    }
}
