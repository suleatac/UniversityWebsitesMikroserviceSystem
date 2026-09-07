using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.ShortcutButtonEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.ShortcutButtonEndPoints
{
    public static class ShortcutButtonEndPointsExt
    {
        public static void AddShortcutButtonGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/shortcutButtons")
                .WithTags("ShortcutButton")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateShortcutButtonEndpointGroupItem();
            group.UpdateShortcutButtonEndpointGroupItem();
            group.DeleteShortcutButtonEndpointGroupItem();
            group.GetShortcutButtonsEndpointGroupItem();
            group.GetShortcutButtonByIdEndpointGroupItem();
            group.RequireAuthorization("ClientCredential");
        }
    }
}
