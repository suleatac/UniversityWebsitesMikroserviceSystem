namespace Microservice.Web.Settings
{
    public class RedisSetting
    {
        public const string Key = "ConnectionStrings";
        public string Redis { get; set; } = default!;
    }
}
