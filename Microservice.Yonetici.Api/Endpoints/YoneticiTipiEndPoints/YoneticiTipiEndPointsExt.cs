using Asp.Versioning.Builder;
using Microservice.Yonetici.Api.Endpoints.YoneticiTipiEndPoints.EndPoints;

namespace Microservice.Yonetici.Api.Endpoints.YoneticiTipiEndPoints
{
    public static class YoneticiTipiEndPointsExt
    {
        public static void AddYoneticiTipiGroupsEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            var group = app.MapGroup("/api/v{version:apiVersion}/yoneticiTipi").WithTags("YoneticiTipi");
            group.CreateYoneticiTipiEndpointGroupItem();
            group.UpdateYoneticiTipiEndpointGroupItem();
            group.DeleteYoneticiTipiEndpointGroupItem();
            group.GetYoneticiTipleriEndpointGroupItem();
            group.WithApiVersionSet(apiVersionSet);
            //group.RequireAuthorization("Instructor");
        }

    }
}
