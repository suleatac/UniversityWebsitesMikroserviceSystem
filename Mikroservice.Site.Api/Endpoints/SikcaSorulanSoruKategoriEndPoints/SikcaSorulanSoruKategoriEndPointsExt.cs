using Asp.Versioning.Builder;
using Mikroservice.Site.Api.Endpoints.SikcaSorulanSoruKategoriEndPoints.EndPoints;

namespace Mikroservice.Site.Api.Endpoints.SikcaSorulanSoruKategoriEndPoints
{
    public static class SikcaSorulanSoruKategoriEndPointsExt
    {
        public static void AddSikcaSorulanSoruKategoriGroupsEndpointExt(
            this WebApplication app,
            ApiVersionSet apiVersionSet)
        {
            var group = app
                .MapGroup("/api/v{version:apiVersion}/sss-kategoriler")
                .WithTags("SikcaSorulanSoruKategori")
                .WithApiVersionSet(apiVersionSet)
                .RequireAuthorization();

            group.MapToApiVersion(1.0);

            group.CreateSikcaSorulanSoruKategoriEndpointGroupItem();
            group.UpdateSikcaSorulanSoruKategoriEndpointGroupItem();
            group.DeleteSikcaSorulanSoruKategoriEndpointGroupItem();
            group.GetSikcaSorulanSoruKategorilerEndpointGroupItem();
            group.GetSikcaSorulanSoruKategoriByIdEndpointGroupItem();
            group.RequireAuthorization("Password");
        }
    }
}
