namespace Microservice.Admin.Settings
{
    public class MinioSetting
    {
        public string Endpoint { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string BucketName { get; set; } = default!;
        public bool UseSSL { get; set; }
    }
}
