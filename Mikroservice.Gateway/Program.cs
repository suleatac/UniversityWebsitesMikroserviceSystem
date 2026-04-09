using AspNetCoreRateLimit;
using Microservice.Shared.Extentions;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Rate limit işlemi için TAMAMEN yapılandırma
builder.Services.AddOptions();
builder.Services.AddMemoryCache();

// Rate limiting servisleri
builder.Services.AddInMemoryRateLimiting();

// Rate limit konfigürasyonları
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

// **EKSİK OLAN SERVİSLERİ EKLEYİN**
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

// **CRITICAL: ProcessingStrategy'yi EKLEYİN**
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

// Rate limit configuration manager
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// HttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();





// CORS politikası
builder.Services.AddCors(opts => {
    opts.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

    opts.AddPolicy("AllowSivasOnly", policy => {
        policy.SetIsOriginAllowed(origin => {
            if (string.IsNullOrEmpty(origin))
                return false;

            var uri = new Uri(origin);
            return uri.Host.EndsWith("sivas.edu.tr");
        })
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Authentication ayarları
builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "YARP (Gateway)!");
app.UseForwardedHeaders(new ForwardedHeadersOptions {
    ForwardedHeaders = ForwardedHeaders.XForwardedFor
});
// Rate limit middleware'ini ekleyin
app.UseIpRateLimiting();

// CORS middleware'i
app.UseCors("AllowSivasOnly");

// Authentication ve Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();
app.Run();