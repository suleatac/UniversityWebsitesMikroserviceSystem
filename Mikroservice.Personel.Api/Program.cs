using Hangfire;
using Hangfire.PostgreSql;
using Logging.Shared;
using Microservice.Personel.Application;
using Microservice.Personel.Application.Contracts.Services;
using Microservice.Shared.Extentions;
using Microservice.Shared.OpenTelemetry;
using Microservice.Shared.SeriLog;
using Microsoft.EntityFrameworkCore;
using Mikroservice.Personel.Api;
using Mikroservice.Personel.Api.Endpoints.Personels.PersonelEndPointExt;
using Mikroservice.Personel.Api.Jobs;
using Mikroservice.Personel.Api.SeedData;
using Mikroservice.Personel.Application.Contracts.Services;
using Mikroservice.Personel.Application.Services;
using Mikroservice.Personel.Persistence;
using Mikroservice.Personel.Persistence.Extentions;
using Mikroservice.Personel.Persistence.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//Authentication ve Authorization servisleri eklendi
builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();





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

// HttpClient
//builder.Services.AddHttpClient();


// HttpClient
//builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IPersonelExternalApiService, PersonelExternalApiService>();
builder.Services.AddScoped<IPersonelSyncService, PersonelSyncService>();
builder.Services.AddScoped<IPersonelSeedService, PersonelSeedService>();

var app = builder.Build();

//çalıştığında otomatik migration yapması için
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}


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
// Hangfire Job Startup
app.Lifetime.ApplicationStarted.Register(() => {
    PersonelHangfireJob.ScheduleDailySync();
});
app.UseMiddleware<OpenTelemetryTraceIdMiddleware>();
app.UseMiddleware<RequestAndResponseActivityMiddleware>();
app.UseExceptionMiddleware();
await app.InitializeSeedDataAsync();
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


