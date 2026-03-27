using MassTransit.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mikroservice.Shared.OpenTelemetry;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;


namespace Microservice.Shared.Extentions
{
    public static class OpenTelemetryExt
    {
        public static IServiceCollection AddOpenTelemetryExt(this IServiceCollection services, IConfiguration configuration)
        {
       

            //Opentelemetry trace için eklendi.
            var OpenTelemetryConstants = configuration.GetSection("OpenTelemetry").Get<OpenTelemetryConstants>();
            
            services.AddOpenTelemetry().WithTracing(tracingBuilder => {
                tracingBuilder
                .AddSource(OpenTelemetryConstants!.ActivitySourceName)
                .AddSource(DiagnosticHeaders.DefaultListenerName) //Masstransit
                .ConfigureResource(resource => {
                    resource.AddService(OpenTelemetryConstants.ServiceName, serviceVersion: OpenTelemetryConstants.ServiceVersion);
                })
                .AddAspNetCoreInstrumentation(options => {
                    options.Filter = static httpContext => {
                        var pathValue = httpContext.Request.Path.Value;

                        // Null veya boş path kontrolü
                        if (string.IsNullOrEmpty(pathValue))
                            return false;

                        // Hangfire dashboard isteklerini hariç tut
                        //if (pathValue.StartsWith("/hangfire", StringComparison.OrdinalIgnoreCase))
                        //    return false;

                        // Sadece 'api' içeren istekleri dahil et
                        return pathValue.Contains("api", StringComparison.OrdinalIgnoreCase);
                    };

                    options.RecordException = true; //bunu true yapınca hata fırlatıldığında trace'e hatanın tüm detayını ekler. false olursa kısaca hatanın açıklamasını ekler.



                })
                .AddNpgsql()//PostgreSql
                .AddHttpClientInstrumentation(httpOptions => {

                    httpOptions.EnrichWithHttpRequestMessage = async (activity, request) => {
                        var requestContent = "empty";

                        if (request.Content != null)
                        {
                            requestContent = await request.Content.ReadAsStringAsync();
                        }


                        activity.SetTag("http.request.body", requestContent);
                    };

                    httpOptions.EnrichWithHttpResponseMessage = async (activity, response) => {

                        if (response.Content != null)
                        {
                            activity.SetTag("http.response.body", await response.Content.ReadAsStringAsync());
                        }

                    };

                })
                .AddRedisInstrumentation(options =>
                {
                    options.SetVerboseDatabaseStatements = true;
                })//Redis
                .SetSampler(new AlwaysOnSampler())
                .AddConsoleExporter()
                .AddOtlpExporter();//jaeger
            });

            var activitySource = new ActivitySource(
                OpenTelemetryConstants!.ActivitySourceName,
                OpenTelemetryConstants.ServiceVersion
            );



            return services;
        }
    }
}
