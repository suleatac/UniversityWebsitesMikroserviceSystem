using Microsoft.Extensions.Logging;
using System.Text;

namespace Mikroservice.Site.Persistence.Services
{
    public class SiteNginxConfigService
    (
        ILogger<SiteNginxConfigService> logger
    )
    {
        private const string ConfDirectory = "/nginx-conf";
        private const string CertificateDirectory = "/etc/nginx/ssl";
        private const string CertificateBaseName = "sivas.edu.tr";

        private const string ProxyPassUrl = "http://microservice.web:8080";//Ana web projesine yönlendirme için kullanılan URL.

        public async Task ApplyAsync(
        string siteAlanAdi,
        string? previousSiteAlanAdi,
        bool isDeleted,
        CancellationToken cancellationToken = default)
        {
      

            if (isDeleted)
            {
                DeleteConfig(GetConfPath(siteAlanAdi));

                if (!string.IsNullOrWhiteSpace(previousSiteAlanAdi))
                {
                    DeleteConfig(GetConfPath(previousSiteAlanAdi));
                }

                return;
            }

            var currentPath = GetConfPath(siteAlanAdi);
            var content = BuildConfigContent(siteAlanAdi);

            Directory.CreateDirectory(ConfDirectory);

            await File.WriteAllTextAsync(
           currentPath,
           content,
           new UTF8Encoding(false),
           cancellationToken);

            logger.LogInformation(
                "Nginx conf yazıldı: {ConfPath}",
                currentPath);

            if (!string.IsNullOrWhiteSpace(previousSiteAlanAdi) &&
                !string.Equals(
                    previousSiteAlanAdi,
                    siteAlanAdi,
                    StringComparison.OrdinalIgnoreCase))
            {
                DeleteConfig(GetConfPath(previousSiteAlanAdi));
            }
        }



        private string GetConfPath(string siteAlanAdi)
        {
            var safeFileName = new string(siteAlanAdi.Select(ch => Path.GetInvalidFileNameChars().Contains(ch) ? '-' : ch).ToArray());

            if (!safeFileName.EndsWith(".conf", StringComparison.OrdinalIgnoreCase))
            {
                safeFileName += ".conf";
            }

            return Path.Combine(ConfDirectory, safeFileName);
        }

        private string BuildConfigContent(string siteAlanAdi)
        {
            return $$"""
            server {
                listen 80;
                server_name {{siteAlanAdi}}.sivas.edu.tr;

                return 301 https://$host$request_uri;
            }

            server {
                listen 443 ssl;
                server_name {{siteAlanAdi}}.sivas.edu.tr;

                ssl_certificate     {{CertificateDirectory}}/{{CertificateBaseName}}.crt;
                ssl_certificate_key {{CertificateDirectory}}/{{CertificateBaseName}}.key;

                ssl_protocols TLSv1.2 TLSv1.3;

                location / {
                    proxy_pass {{ProxyPassUrl}};
                    proxy_set_header Host $host;
                    proxy_set_header X-Real-IP $remote_addr;
                    proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
                    proxy_set_header X-Forwarded-Proto https;
                }
            }
            """;
        }

        private bool DeleteConfig(string confPath)
        {
            if (!File.Exists(confPath))
            {
                logger.LogInformation("Silinecek nginx conf bulunamadı: {ConfPath}", confPath);
                return false;
            }

            File.Delete(confPath);
            logger.LogInformation("Nginx conf silindi: {ConfPath}", confPath);
            return true;
        }


    }
}