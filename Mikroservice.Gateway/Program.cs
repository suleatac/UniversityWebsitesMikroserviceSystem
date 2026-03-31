using AspNetCoreRateLimit;
using Microservice.Shared.Extentions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

//rate limit işlemi için eklenen kısım
builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



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



//Authentication ayarları
builder.Services.AddAuthenticationAndAuthorizationExt(builder.Configuration);



var app = builder.Build();

//rate limit middleware'ini ekleyelim
app.UseIpRateLimiting();
//Gerçek IP’yi Almak için Forwarded Headers Middleware’i Ekleyelim
app.UseForwardedHeaders();

app.UseExceptionHandler(x => { });
app.MapReverseProxy();

app.MapGet("/", () => "YARP (Gateway)!");


//CORS middleware'i eklendi
//app.UseCors();
app.UseCors("AllowSivasOnly");


//Authentication ve Authorization middleware'lerini ekleyelim
app.UseAuthentication();
app.UseAuthorization();
app.Run();
