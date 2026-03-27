namespace Mikroservice.Shared.OpenTelemetry
{
    public class OpenTelemetryConstants
    {

        public string ServiceName { get; set; } = null!;
        public string ServiceVersion { get; set; } = null!;
        public string ActivitySourceName { get; set; } = null!;
        public string ActivitySourceFileName { get; set; } = null!;

    }
}
