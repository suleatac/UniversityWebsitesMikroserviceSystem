using AspNetCoreRateLimit;
using Hangfire;
using Hangfire.PostgreSql;
using Logging.Shared;
using Microservice.Ogrenci.Application;
using Microservice.Shared.Extentions;
using Microservice.Shared.OpenTelemetry;
using Microservice.Shared.SeriLog;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Ogrenci.Api;
using Mikroservice.Ogrenci.Api.Endpoints.OgrenciEndPoints.OgrenciEndPoints;
using Mikroservice.Ogrenci.Api.Jobs;
using Mikroservice.Ogrenci.Api.SeedData;
using Mikroservice.Ogrenci.Application.Contracts.Services;
using Mikroservice.Ogrenci.Application.Services;
using Mikroservice.Ogrenci.Persistence;
using Mikroservice.Ogrenci.Persistence.Extentions;
using Mikroservice.Ogrenci.Persistence.Services;
using Serilog;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//CORS politikası eklendi
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

            // sivas.edu.tr veya alt domainleri
            return uri.Host.EndsWith("sivas.edu.tr");
        })
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


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




//Authentication ve Authorization servisleri eklendi
builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);

//Trace işlemi için eklenen extentionlar
builder.Services.AddOpenTelemetryTraceExt(builder.Configuration);

//Log işlemi için eklenen kısım
builder.Host.UseSerilog(Microservice.Shared.SeriLog.Logging.ConfigureLogging);

builder.Services.AddPersistenceExtentions(builder.Configuration);
builder.Services.AddCommonServiceExt(typeof(OgrenciApplicationAssembly));

builder.Services.AddRedisCacheExt(builder.Configuration);
//Hangfire ayarları
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(c => {
        var connectionToString = builder.Configuration.GetSection(HangFireConnectionToString.Key).Get<HangFireConnectionToString>();

        c.UseNpgsqlConnection(connectionToString!.HangfirePostgreSqlServer);

    }));


builder.Services.AddHangfireServer();
// HttpClient
//builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IOgrenciExternalApiService, OgrenciExternalApiService>();
builder.Services.AddScoped<IOgrenciSyncService, OgrenciSyncService>();
builder.Services.AddScoped<IOgrenciSeedService, OgrenciSeedService>();


//Versiyonlama eklendi
builder.Services.AddVersioningExt();







//builder.Services.AddScoped<OgrenciRecurringJob>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

//çalıştığında otomatik migration yapması için
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}



//Ip Rate Limiting middleware'i eklendi
app.UseIpRateLimiting();
//CORS middleware'i eklendi
//app.UseCors();
app.UseCors("AllowSivasOnly");
//Authentication ve Authorization middleware'leri eklendi
app.UseAuthentication();
app.UseAuthorization();

//endpoint grupları eklendi
app.AddOgrenciGroupEndpointExt(app.AddVersionSetExt());

app.UseHangfireDashboard("/hangfire", new DashboardOptions {
    Authorization = new[] { new AllowAll() }
});

// Hangfire Job Startup
app.Lifetime.ApplicationStarted.Register(() => {
OgrenciHangfireJob.ScheduleDailySync();
});
app.UseMiddleware<OpenTelemetryTraceIdMiddleware>();
app.UseMiddleware<RequestAndResponseActivityMiddleware>();
app.UseExceptionMiddleware();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitializeSeedDataAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}
// Production'da da çalışsın ama sadece boş DB'de
else if (!app.Environment.IsProduction())
{
    await app.InitializeSeedDataAsync();
}
//Metric işlemi için eklenen middleware
app.UseOpenTelemetryPrometheusScrapingEndpoint("/metrics");
app.Run();


