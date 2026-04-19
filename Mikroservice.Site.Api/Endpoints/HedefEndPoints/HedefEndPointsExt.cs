using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.HedefEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.HedefEndPoints
{
    public static class HedefEndPointsExt
    {
        public static void AddHedefGroupsEndpointExt(
   this WebApplication app,
   ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/hedefler")
                .WithTags("Hedef")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);
            group.GetHedefsEndpointGroupItem();


            group.RequireAuthorization("Password");
        }
    }
}
