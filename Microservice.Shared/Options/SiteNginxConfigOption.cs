namespace Microservice.Shared.Options
{
    public class SiteNginxConfigOption
    {
        public const string Key = "SiteNginxConfig";

        public string ProxyPassUrl { get; set; } = default!;
        public string? ReloadCommand { get; set; }
        public string? ReloadArguments { get; set; }
    }
}