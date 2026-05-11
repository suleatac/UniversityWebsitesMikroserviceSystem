using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.MenuEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.MenuEndPoints
{
    public static class MenuEndPointsExt
    {
        public static void AddMenuGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/menus")
                .WithTags("Menu")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateMenuEndpointGroupItem();
            group.UpdateMenuEndpointGroupItem();
            group.DeleteMenuEndpointGroupItem();
            group.GetMenusEndpointGroupItem();
            group.GetMenuByIdEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
