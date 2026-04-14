using Asp.Versioning.Builder;
using Microservice.Site.Api.Endpoints.YoneticiDuyuruEndPoints.EndPoints;

namespace Microservice.Site.Api.Endpoints.YoneticiDuyuruEndPoints
{
    public static class YoneticiDuyuruEndPointsExt
    {
        public static void AddYoneticiDuyuruGroupsEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            var group = app.MapGroup("/api/v{version:apiVersion}/yoneticiDuyuru").WithTags("YoneticiDuyuru");
            group.CreateYoneticiDuyuruEndpointGroupItem();
            group.UpdateYoneticiDuyuruEndpointGroupItem();
            group.DeleteYoneticiDuyuruEndpointGroupItem();
            group.GetYoneticiDuyurulariEndpointGroupItem();
            group.WithApiVersionSet(apiVersionSet);
            //group.RequireAuthorization("Instructor");
        }
    }
}
