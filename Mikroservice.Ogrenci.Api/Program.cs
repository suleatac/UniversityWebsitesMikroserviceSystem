using Hangfire;
using Hangfire.PostgreSql;
using Microservice.Ogrenci.Application;
using Microservice.Shared.Extentions;
using Mikroservice.Ogrenci.Api;
using Mikroservice.Ogrenci.Api.Endpoints.OgrenciEndPoints.OgrenciEndPoints;
using Mikroservice.Ogrenci.Persistence.Extentions;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCommonServiceExt(typeof(OgrenciApplicationAssembly));
builder.Services.AddPersistenceExtentions(builder.Configuration);

//Hangfire ayarları
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(c => {
        var connectionToString = builder.Configuration.GetSection(HangFireConnectionToString.Key).Get<HangFireConnectionToString>();

        c.UseNpgsqlConnection(connectionToString!.HangfirePostgreSqlServer);

    }));


builder.Services.AddHangfireServer();

builder.Services.AddHttpClient();
builder.Services.AddScoped<OgrenciRecurringJob>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
app.AddOgrenciGroupEndpointExt();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard("/hangfire");
    OgrenciRecurringJob.VeriTabaniGuncellemeJob();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.Run();


