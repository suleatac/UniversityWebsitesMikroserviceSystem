using AspNetCoreRateLimit;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Logging.Shared;
using Microservice.Personel.Application;
using Microservice.Shared.Extentions;
using Microservice.Shared.OpenTelemetry;
using Microservice.Shared.SeriLog;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Personel.Api;
using Mikroservice.Personel.Api.Endpoints.Personels.PersonelEndPointExt;
using Mikroservice.Personel.Api.RecurringJob;
using Mikroservice.Personel.Persistence;
using Mikroservice.Personel.Persistence.Extentions;

using Serilog;

var builder = WebApplication.CreateBuilder(args);
//Authentication ve Authorization servisleri eklendi
builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//CORS politikası eklendi
builder.Services.AddCors(opts => {

    opts.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

    opts.AddPolicy("AllowSivasOnly", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
        {
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
//rate limit işlemi için eklenen kısım
builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCommonServiceExt(typeof(PersonelApplicationAssembly));
builder.Services.AddPersistenceExtentions(builder.Configuration);
builder.Services.AddRedisCacheExt(builder.Configuration);
//Hangfire ayarları
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(c => {
        var connectionToString = builder.Configuration.GetSection(HangFireConnectionToString.Key).Get<HangFireConnectionToString>();

        c.UseNpgsqlConnection(connectionToString!.HangfirePostgreSqlServer);

    }));

builder.Services.AddHangfireServer();

//Trace işlemi için eklenen extentionlar
builder.Services.AddOpenTelemetryTraceExt(builder.Configuration);

//Log işlemi için eklenen kısım
builder.Host.UseSerilog(Microservice.Shared.SeriLog.Logging.ConfigureLogging);

//Versiyonlama eklendi
builder.Services.AddVersioningExt();



builder.Services.AddHttpClient();
builder.Services.AddScoped<PersonelRecurringJob>();

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

app.AddPersonelGroupEndpointExt(app.AddVersionSetExt());
app.UseHangfireDashboard("/hangfire", new DashboardOptions {
    Authorization = new[] { new AllowAll() }
});
PersonelRecurringJob.VeriTabaniGuncellemeJob();
app.UseMiddleware<OpenTelemetryTraceIdMiddleware>();
app.UseMiddleware<RequestAndResponseActivityMiddleware>();
app.UseExceptionMiddleware();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}
//Metric işlemi için eklenen middleware
app.UseOpenTelemetryPrometheusScrapingEndpoint("/metrics");
app.Run();


