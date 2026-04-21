using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.MediaFileEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.MediaFileEndPoints
{
    public static class MediaFileEndPointExt
    {
        public static void AddMediaFileGroupsEndpointExt(
           this WebApplication app,
           ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/mediafiles")
                .WithTags("MediaFile")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateMediaFileEndpointGroupItem();
            group.UpdateMediaFileEndpointGroupItem();
            group.DeleteMediaFileEndpointGroupItem();
            group.GetMediaFileEndpointGroupItem();

            group.RequireAuthorization("Password");
        }
    }
}
