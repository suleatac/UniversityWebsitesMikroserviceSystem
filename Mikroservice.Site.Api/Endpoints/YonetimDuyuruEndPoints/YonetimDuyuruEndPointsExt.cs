using Asp.Versioning.Builder;
using Microservice.Site.Api.Endpoints.YonetimDuyuruEndPoints.EndPoints;
using Mikroservice.Site.Api.Endpoints.YonetimDuyuruEndPoints.EndPoints;

namespace Microservice.Site.Api.Endpoints.YonetimDuyuruEndPoints
{
    public static class YonetimDuyuruEndPointsExt
    {
        public static void AddYonetimDuyuruGroupsEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            var group = app.MapGroup("/api/v{version:apiVersion}/yonetimDuyuru").WithTags("YonetimDuyuru");
            group.CreateYonetimDuyuruEndpointGroupItem();
            group.UpdateYonetimDuyuruEndpointGroupItem();
            group.DeleteYonetimDuyuruEndpointGroupItem();
            group.GetYonetimDuyurulariEndpointGroupItem();
            group.GetPaginatedYonetimDuyuruEndpointGroupItem();
            group.WithApiVersionSet(apiVersionSet);
            group.RequireAuthorization("Instructor");
        }
    }
}
