using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.BannerEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.BannerEndPoints
{
    public static class BannerEndPointsExt
    {
        public static void AddBannerGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/banners")
                .WithTags("Banner")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateBannerEndpointGroupItem();
            group.UpdateBannerEndpointGroupItem();
            group.DeleteBannerEndpointGroupItem();
            group.GetBannersEndpointGroupItem();
            group.GetBannerByIdEndpointGroupItem();
            group.GetPaginatedBannerEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
