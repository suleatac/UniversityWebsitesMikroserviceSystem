using Asp.Versioning.Builder;

namespace Mikroservice.Personel.Api.Endpoints.Personels.PersonelEndPointExt
{
    public static class PersonelEndpointExtentions
    {
        public static void AddPersonelGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
        {
            var group = app.MapGroup("/api/v{version:apiVersion}/personels").WithTags("Personels");
            group.GetPersonelsGroupItemEndpoint();
            group.WithApiVersionSet(apiVersionSet);
            //group.RequireAuthorization("Password");
        }
    }
}
