using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.PersonelTipEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.PersonelTipEndPoints
{
    public static class PersonelTipEndPointsExt
    {
        public static void AddPersonelTipGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/personel-tipler")
                .WithTags("PersonelTip")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreatePersonelTipEndpointGroupItem();
            group.UpdatePersonelTipEndpointGroupItem();
            group.DeletePersonelTipEndpointGroupItem();
            group.GetPersonelTiplerEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
