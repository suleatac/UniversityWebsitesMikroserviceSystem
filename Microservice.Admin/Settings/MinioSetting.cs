namespace Microservice.Admin.Settings
{
    public class MinioSetting
    {
        public string Endpoint { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string BucketName { get; set; } = default!;
        public bool UseSSL { get; set; }

        /// <summary>
        /// Iceriklere kaydedilecek goreceli yol on eki (nginx uzerinden MinIO'ya proxy'lenir).
        /// Ornek: "/media/" -> /media/site/1/haber/abc.jpg
        /// </summary>
        public string PublicPrefix { get; set; } = "/media/";

        /// <summary>
        /// Yalnizca admin panelinin kendi onizlemeleri icin kullanilan erisilebilir MinIO adresi.
        /// Bos birakilirsa Endpoint + BucketName'den uretilir.
        /// Ornek: "http://localhost:9000/site-media/"
        /// </summary>
        public string? PreviewBaseUrl { get; set; }
    }
}
