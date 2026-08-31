using System.Diagnostics;
using System.Text;
using Microservice.Shared.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mikroservice.Site.Persistence.Services
{
    public class SiteNginxConfigService
    (
        IOptions<SiteNginxConfigOption> options,
        ILogger<SiteNginxConfigService> logger
    )
    {
        private const string ConfDirectory = "/nginx-conf";
        private const string CertificateDirectory = "/etc/nginx/ssl";
        private const string CertificateBaseName = "sivas.edu.tr";

        private readonly SiteNginxConfigOption _options = options.Value;

        public async Task ApplyAsync(string siteAlanAdi, string? previousSiteAlanAdi, bool isDeleted, CancellationToken cancellationToken = default)
        {
            ValidateOptions();

            var changed = false;

            if (isDeleted)
            {
                changed = DeleteConfig(GetConfPath(siteAlanAdi));
            }
            else
            {
                var currentPath = GetConfPath(siteAlanAdi);
                var content = BuildConfigContent(siteAlanAdi);

                Directory.CreateDirectory(ConfDirectory);
                await File.WriteAllTextAsync(currentPath, content, Encoding.UTF8, cancellationToken);
                logger.LogInformation("Nginx conf yazıldı: {ConfPath}", currentPath);

                if (!string.IsNullOrWhiteSpace(previousSiteAlanAdi) &&
                    !string.Equals(previousSiteAlanAdi, siteAlanAdi, StringComparison.OrdinalIgnoreCase))
                {
                    changed |= DeleteConfig(GetConfPath(previousSiteAlanAdi));
                }

                changed = true;
            }

            if (!changed)
            {
                logger.LogInformation("Nginx conf değişmedi. SiteAlanAdi: {SiteAlanAdi}", siteAlanAdi);
                return;
            }

            await ReloadAsync(cancellationToken);
        }

        private void ValidateOptions()
        {
            if (string.IsNullOrWhiteSpace(_options.ProxyPassUrl))
                throw new InvalidOperationException("SiteNginxConfig:ProxyPassUrl tanımlı değil.");
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
                    proxy_pass {{_options.ProxyPassUrl}};
                    proxy_ssl_verify off;
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

        private async Task ReloadAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_options.ReloadCommand))
            {
                logger.LogWarning("ReloadCommand tanımlı değil. Nginx reload atlanıyor.");
                return;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = _options.ReloadCommand,
                Arguments = _options.ReloadArguments ?? string.Empty,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Nginx reload komutu başlatılamadı.");

            await process.WaitForExitAsync(cancellationToken);

            var stdout = await process.StandardOutput.ReadToEndAsync(cancellationToken);
            var stderr = await process.StandardError.ReadToEndAsync(cancellationToken);

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"Nginx reload başarısız oldu. ExitCode: {process.ExitCode}. Error: {stderr}");
            }

            logger.LogInformation("Nginx reload tamamlandı. Output: {Output}", stdout);
        }
    }
}