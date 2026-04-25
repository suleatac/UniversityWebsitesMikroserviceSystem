using Microservice.Admin.Configurations;
using Microservice.Admin.Services;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Settings;
using Microsoft.Extensions.Options;
using Minio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();




builder.Services.AddMinioExtentions(builder.Configuration);

builder.Services.AddScoped<IMediaService, MediaService>();


//Identity Server Configuration
builder.Services.AddOptions<IdentitySetting>()
              .BindConfiguration(IdentitySetting.SectionName)
              .ValidateDataAnnotations()
              .ValidateOnStart();

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<IdentitySetting>>().Value);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
