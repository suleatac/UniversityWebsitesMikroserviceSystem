using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.TemplateEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.TemplateEndPoints
{
    public static class TemplateEndPointsExt
    {
        public static void AddTemplateGroupsEndpointExt(
      this WebApplication app,
      ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/templates")
                .WithTags("Template")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);
            group.GetTemplatesEndpointGroupItem();


            group.RequireAuthorization("Password");
        }
    }
}
