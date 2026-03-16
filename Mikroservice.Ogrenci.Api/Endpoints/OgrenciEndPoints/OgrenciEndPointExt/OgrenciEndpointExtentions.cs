namespace Mikroservice.Ogrenci.Api.Endpoints.OgrenciEndPoints.OgrenciEndPoints
{
    public static class OgrenciEndpointExtentions
    {
        public static void AddOgrenciGroupEndpointExt(this WebApplication app)
        {
            var group = app.MapGroup("/api/ogrencis").WithTags("Ogrencis");
            group.GetOgrencisGroupItemEndpoint();
        }
    }
}
