using Hangfire;
using Hangfire.PostgreSql;
using Microservice.Ogrenci.Application;
using Microservice.Shared.Extentions;
using Microservice.Shared.OpenTelemetry;
using Mikroservice.Ogrenci.Api;
using Mikroservice.Ogrenci.Api.Endpoints.OgrenciEndPoints.OgrenciEndPoints;
using Mikroservice.Ogrenci.Api.RecurringJob;
using Mikroservice.Ogrenci.Persistence.Extentions;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Authentication ve Authorization servisleri eklendi
builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);

//Trace işlemi için eklenen extentionlar
builder.Services.AddOpenTelemetryExt(builder.Configuration);



builder.Services.AddCommonServiceExt(typeof(OgrenciApplicationAssembly));
builder.Services.AddPersistenceExtentions(builder.Configuration);
builder.Services.AddRedisCacheExt(builder.Configuration);
//Hangfire ayarları
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(c => {
        var connectionToString = builder.Configuration.GetSection(HangFireConnectionToString.Key).Get<HangFireConnectionToString>();

        c.UseNpgsqlConnection(connectionToString!.HangfirePostgreSqlServer);

    }));


builder.Services.AddHangfireServer();
//Versiyonlama eklendi
builder.Services.AddVersioningExt();




builder.Services.AddHttpClient();
builder.Services.AddScoped<OgrenciRecurringJob>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

//çalıştığında otomatik migration yapması için
//using (var scope = app.Services.CreateScope())
//{
//    var serviceProvider = scope.ServiceProvider;
//    var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
//    await dbContext.Database.MigrateAsync();
//}



app.UseAuthentication();
app.UseAuthorization();
//endpoint grupları eklendi
app.AddOgrenciGroupEndpointExt(app.AddVersionSetExt());

app.UseHangfireDashboard("/hangfire", new DashboardOptions {
    Authorization = new[] { new AllowAll() }
});
OgrenciRecurringJob.VeriTabaniGuncellemeJob();

app.UseMiddleware<RequestAndResponseActivityMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.Run();


