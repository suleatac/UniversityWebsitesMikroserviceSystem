using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.SikcaSorulanSoruEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.SikcaSorulanSoruEndPoints
{
    public static class SikcaSorulanSoruEndPointsExt
    {
        public static void AddSikcaSorulanSoruGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/sss")
                .WithTags("SikcaSorulanSoru")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateSikcaSorulanSoruEndpointGroupItem();
            group.UpdateSikcaSorulanSoruEndpointGroupItem();
            group.DeleteSikcaSorulanSoruEndpointGroupItem();
            group.GetSikcaSorulanSorularEndpointGroupItem();
            group.GetSikcaSorulanSoruByIdEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
