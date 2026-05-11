using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.VideoEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.VideoEndPoints
{
    public static class VideoEndPointsExt
    {
        public static void AddVideoGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/videos")
                .WithTags("Video")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateVideoEndpointGroupItem();
            group.UpdateVideoEndpointGroupItem();
            group.DeleteVideoEndpointGroupItem();
            group.GetVideosEndpointGroupItem();
            group.GetVideoByIdEndpointGroupItem();
            group.GetPaginatedVideoEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
