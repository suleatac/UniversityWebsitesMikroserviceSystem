using Microservice.Web.Clients;
using Microservice.Web.Configurations;
using Microservice.Web.HttpHandlers;
using Microservice.Web.Messaging;
using Microservice.Web.SeriLog;
using Microservice.Web.Services.ServicesExtentions;
using Microsoft.AspNetCore.Localization;
using Serilog;
using Smidge;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// SignalR: Hangfire senkronizasyonlari tamamlanınca sayfaları bilgilendirmek için
builder.Services.AddSignalR();


//Configuration Ayarları
builder.Services.AddIdentityServerExtentions(builder.Configuration);
builder.Services.AddMicroservicesConfiguration(builder.Configuration);
builder.Services.AddRedisExtentions(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();


//Http handler Ayarları
builder.Services.AddScoped<ClientAuthenticatedHttpClientHandler>();

//Services Ayarları
builder.Services.AddServicesExtentions(builder.Configuration);

//Client Extentions Ayarları
builder.Services.AddClientExtentions(builder.Configuration);

//Rabbitmq consumer (Personel/Ogrenci sync event'leri -> Redis cache invalidation + SignalR broadcast)
builder.Services.AddRabbitmqConsumerExtensions(builder.Configuration);

//Logging Extentions Ayarları
builder.Services.AddLoggingExt(builder.Configuration);
//Log işlemi için eklenen kısım
builder.Host.UseSerilog(Logging.ConfigureLogging);


//Smidge ile bundling işlemi
builder.Services.AddSmidge(builder.Configuration.GetSection("Smidge"));




var app = builder.Build();

//app.UseExceptionMiddleware();

var cultueInfo = new CultureInfo("tr-TR");
CultureInfo.DefaultThreadCurrentCulture = cultueInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultueInfo;
app.UseRequestLocalization(new RequestLocalizationOptions {
    DefaultRequestCulture = new RequestCulture(cultueInfo),
    SupportedCultures = new List<CultureInfo> { cultueInfo },
    SupportedUICultures = new List<CultureInfo> { cultueInfo }
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseRouting();

app.MapStaticAssets();

// SignalR hub endpoint'i (data-refresh.js bu adrese bağlanır)
app.MapHub<Microservice.Web.Hubs.DataRefreshHub>("/hubs/data-refresh");

// Attribute routed controller'lar (IletisimController gibi) catch-all'dan once eslesmeli
app.MapControllers();

//Smidge Bundling Settings
app.UseSmidge(bundle => {
    bundle.CreateCss("template1-css-bundle",
     "~/template1/css/bootstrap.css",
     "~/template1/css/font-awesome.css",
     "~/template1/css/icomoon.css",
     "~/template1/css/flexslider.css",
     "~/template1/css/owl.carousel.css",
     "~/template1/css/revolution-slider.css",
     "~/template1/css/jquery.fancybox.css",
     "~/template1/css/shortcodes.css",
     "~/template1/css/style.css",
     "~/template1/css/responsive.css",
     "~/template1/css/animate.css"

     //Not: colors/color1.css bundle'a dahil edilmedi. switcher.js, <link id="colors">
     //etiketinin href'ini dinamik degistirdigi icin bu dosya Layout'da ayri link olarak kaliyor.
     );
    bundle.CreateJs("template1-js-bundle",
        "~/template1/js/jquery.min.js",
        "~/template1/js/bootstrap.min.js",
        "~/template1/js/jquery.tweet.min.js",
        "~/template1/js/jquery.matchHeight-min.js",
        "~/template1/js/jquery.easing.js",
        "~/template1/js/jQuery.verticalCarousel.js",
        "~/template1/js/owl.carousel.js",
        "~/template1/js/parallax.js",
        "~/template1/js/jquery-waypoints.js",
        "~/template1/js/jquery-validate.js",
        "~/template1/js/jquery.themepunch.tools.min.js",
        "~/template1/js/jquery.themepunch.revolution.min.js",
        "~/template1/js/slider.js",
        "~/template1/js/jquery.flexslider.js",
        "~/template1/js/jquery.bxslider.js",
        "~/template1/js/imagesloaded.min.js",
        "~/template1/js/masonry.pkgd.js",
        "~/template1/js/jquery.isotope.min.js",
        "~/template1/js/jquery.fancybox.js",
        "~/template1/js/jquery.easypiechart.min.js",
        "~/template1/js/jquery-countTo.js",
        "~/template1/js/jquery.fitvids.js",
        "~/template1/js/jquery.sticky.js",
        "~/template1/js/smoothscroll.js",
        "~/template1/js/gmap3.min.js",
        "~/template1/js/jquery.cookie.js",
        "~/template1/js/switcher.js",
        "~/template1/js/main.js"
        );

    bundle.CreateCss("template2-css-bundle",
        "~/template2/css/animate.css",
        "~/template2/bootstrap/css/bootstrap.min.css",
        "~/template2/css/ionicons.min.css",
        "~/template2/css/themify-icons.css",
        "~/template2/css/all.min.css",
        "~/template2/owlcarousel/css/owl.carousel.min.css",
        "~/template2/owlcarousel/css/owl.theme.css",
        "~/template2/owlcarousel/css/owl.theme.default.min.css",
        "~/template2/css/magnific-popup.css",
        "~/template2/css/style.css",
        "~/template2/css/responsive.css"

        //Not: color/theme-*.css bundle'a dahil edilmedi. Layout'da <link id="layoutstyle">
        //olarak ayri kaliyor (tema rengi degisikliklerine acik birakildi).
        );
    bundle.CreateJs("template2-js-bundle",
        "~/template2/js/jquery-1.12.4.min.js",
        "~/template2/js/jquery-ui.js",
        "~/template2/js/popper.min.js",
        "~/template2/bootstrap/js/bootstrap.min.js",
        "~/template2/owlcarousel/js/owl.carousel.min.js",
        "~/template2/js/magnific-popup.min.js",
        "~/template2/js/waypoints.min.js",
        "~/template2/js/parallax.js",
        "~/template2/js/jquery.countdown.min.js",
        "~/template2/js/jquery.counterup.min.js",
        "~/template2/js/imagesloaded.pkgd.min.js",
        "~/template2/js/isotope.min.js",
        "~/template2/js/jquery.parallax-scroll.js",
        "~/template2/js/scripts.js"
        );

    bundle.CreateCss("template3-css-bundle",
        "~/template3/css/animate.css",
        "~/template3/bootstrap/css/bootstrap.min.css",
        "~/template3/css/ionicons.min.css",
        "~/template3/css/themify-icons.css",
        "~/template3/css/all.min.css",
        "~/template3/owlcarousel/css/owl.carousel.min.css",
        "~/template3/owlcarousel/css/owl.theme.css",
        "~/template3/owlcarousel/css/owl.theme.default.min.css",
        "~/template3/css/magnific-popup.css",
        "~/template3/css/style.css",
        "~/template3/css/responsive.css"

        //Not: color/theme-*.css bundle'a dahil edilmedi. Layout'da <link id="layoutstyle">
        //olarak ayri kaliyor (tema rengi degisikliklerine acik birakildi).
        );
    bundle.CreateJs("template3-js-bundle",
        "~/template3/js/jquery-1.12.4.min.js",
        "~/template3/js/jquery-ui.js",
        "~/template3/js/popper.min.js",
        "~/template3/bootstrap/js/bootstrap.min.js",
        "~/template3/owlcarousel/js/owl.carousel.min.js",
        "~/template3/js/magnific-popup.min.js",
        "~/template3/js/waypoints.min.js",
        "~/template3/js/parallax.js",
        "~/template3/js/jquery.countdown.min.js",
        "~/template3/js/jquery.counterup.min.js",
        "~/template3/js/imagesloaded.pkgd.min.js",
        "~/template3/js/isotope.min.js",
        "~/template3/js/jquery.parallax-scroll.js",
        "~/template3/js/scripts.js"
        );

    bundle.CreateCss("template4-css-bundle",
        "~/template4/css/animate.css",
        "~/template4/bootstrap/css/bootstrap.min.css",
        "~/template4/css/ionicons.min.css",
        "~/template4/css/themify-icons.css",
        "~/template4/css/all.min.css",
        "~/template4/owlcarousel/css/owl.carousel.min.css",
        "~/template4/owlcarousel/css/owl.theme.css",
        "~/template4/owlcarousel/css/owl.theme.default.min.css",
        "~/template4/css/magnific-popup.css",
        "~/template4/css/style.css",
        "~/template4/css/responsive.css"

        //Not: color/theme-*.css bundle'a dahil edilmedi. Layout'da <link id="layoutstyle">
        //olarak ayri kaliyor (tema rengi degisikliklerine acik birakildi).
        );
    bundle.CreateJs("template4-js-bundle",
        "~/template4/js/jquery-1.12.4.min.js",
        "~/template4/js/jquery-ui.js",
        "~/template4/js/popper.min.js",
        "~/template4/bootstrap/js/bootstrap.min.js",
        "~/template4/owlcarousel/js/owl.carousel.min.js",
        "~/template4/js/magnific-popup.min.js",
        "~/template4/js/waypoints.min.js",
        "~/template4/js/parallax.js",
        "~/template4/js/jquery.countdown.min.js",
        "~/template4/js/jquery.counterup.min.js",
        "~/template4/js/imagesloaded.pkgd.min.js",
        "~/template4/js/isotope.min.js",
        "~/template4/js/jquery.parallax-scroll.js",
        "~/template4/js/scripts.js"
        );
 


});
// catch-all'dan önce eşleşmeli, yoksa her zaman Template/Index'e düşer
app.MapControllerRoute(
    name: "error",
    pattern: "/Error",
    defaults: new {
        controller = "Template",
        action = "Error"
    });

app.MapControllerRoute(
    name: "template",
    pattern: "{*path}",
    defaults: new {
        controller = "Template",
        action = "Index"
    });

app.Run();
