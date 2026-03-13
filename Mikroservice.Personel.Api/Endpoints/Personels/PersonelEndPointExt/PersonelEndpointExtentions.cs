namespace Mikroservice.Personel.Api.Endpoints.Personels.PersonelEndPointExt
{
    public static class PersonelEndpointExtentions
    {
        public static void AddPersonelGroupEndpointExt(this WebApplication app)
        {
            var group = app.MapGroup("/api/personels").WithTags("Personels");
            group.GetPersonelsGroupItemEndpoint();
        }
    }
}
