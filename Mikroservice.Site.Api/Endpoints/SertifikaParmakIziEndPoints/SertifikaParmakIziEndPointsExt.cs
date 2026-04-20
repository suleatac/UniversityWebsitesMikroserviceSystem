using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.SertifikaParmakIziEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.SertifikaParmakIziEndPoints
{
    public static class SertifikaParmakIziEndPointsExt
    {
        public static void AddSertifikaParmakIziGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/sertifika-parmak-izleri")
                .WithTags("SertifikaParmakIzi")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateSertifikaParmakIziEndpointGroupItem();
            group.UpdateSertifikaParmakIziEndpointGroupItem();
            group.DeleteSertifikaParmakIziEndpointGroupItem();
            group.GetSertifikaParmakIzleriEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
