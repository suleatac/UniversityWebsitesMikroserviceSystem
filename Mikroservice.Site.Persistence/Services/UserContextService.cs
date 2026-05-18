using Microsoft.AspNetCore.Http;
using Mikroservice.Site.Application.Contracts.Services;
using System.Security.Claims;

namespace Mikroservice.Site.Persistence.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // JWT Token içindeki "UserId" claim'ini okur
        public int UserId => int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value ?? "0");

        // JWT Token içindeki "Username" veya "sub" claim'ini okur
        public string? Username => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value
                                   ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value
                                   ?? _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        // OpenTelemetry veya Middleware tarafından oluşturulan TraceId'yi okur
        public string? TraceId => System.Diagnostics.Activity.Current?.TraceId.ToString()
                                  ?? _httpContextAccessor.HttpContext?.TraceIdentifier;

        // X-Forwarded-For header'ı varsa onu, yoksa RemoteIpAddress'i döndürür
        public string? IpAddress
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null) return null;

                var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(forwardedFor))
                {
                    // X-Forwarded-For birden fazla IP içerebilir (proxy zinciri), ilkini alırız
                    return forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault();
                }

                return httpContext.Connection.RemoteIpAddress?.ToString();
            }
        }
    }
}
