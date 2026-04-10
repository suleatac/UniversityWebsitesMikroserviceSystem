namespace Mikroservice.Yonetici.Persistence
{
    public class ConnectionTostringOption
    {
        public const string Key = "ConnectionStrings";
        public string PostgreSqlServer { get; set; } = default!;
    }
}
