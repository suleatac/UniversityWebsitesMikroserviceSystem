using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.UnvanEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.UnvanEndPoints
{
    public static class UnvanEndPointsExt
    {
        public static void AddUnvanGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/unvans")
                .WithTags("Unvan")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateUnvanEndpointGroupItem();
            group.UpdateUnvanEndpointGroupItem();
            group.DeleteUnvanEndpointGroupItem();
            group.GetUnvanlarEndpointGroupItem();
            group.GetUnvanByIdEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
