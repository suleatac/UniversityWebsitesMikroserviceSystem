using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.PopupEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.PopupEndPoints
{
    public static class PopupEndPointsExt
    {
        public static void AddPopupGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/popups")
                .WithTags("Popup")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreatePopupEndpointGroupItem();
            group.UpdatePopupEndpointGroupItem();
            group.DeletePopupEndpointGroupItem();
            group.GetPopupsEndpointGroupItem();
            group.GetPopupByIdEndpointGroupItem();
            group.GetPaginatedPopupEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
