namespace Mikroservice.Ogrenci.Api
{
    public class HangFireConnectionToString
    {
        public const string Key = "ConnectionStrings";
        public string HangfirePostgreSqlServer { get; set; } = default!;
    }
}
