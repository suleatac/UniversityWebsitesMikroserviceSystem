using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.EtkinlikEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.EtkinlikEndPoints
{
    public static class EtkinlikEndPointsExt
    {
        public static void AddEtkinlikGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/etkinlikler")
                .WithTags("Etkinlik")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateEtkinlikEndpointGroupItem();
            group.UpdateEtkinlikEndpointGroupItem();
            group.DeleteEtkinlikEndpointGroupItem();
            group.GetEtkinliklerEndpointGroupItem();
            group.GetEtkinlikByIdEndpointGroupItem();
            group.GetPaginatedEtkinlikEndpointGroupItem();

            group.RequireAuthorization("Password");
        }
    }
}
