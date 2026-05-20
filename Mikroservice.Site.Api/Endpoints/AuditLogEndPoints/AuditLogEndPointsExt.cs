using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.AuditLogEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.AuditLogEndPoints
{
    public static class AuditLogEndPointsExt
    {
        public static void AddAuditLogGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/audit-logs")
                .WithTags("AuditLog")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateAuditLogEndpointGroupItem();
            group.DeleteAuditLogEndpointGroupItem();
            group.GetAuditLogByIdEndpointGroupItem();
            group.GetPaginatedAuditLogEndpointGroupItem();
            group.GetAuditLogDailyStatsEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
