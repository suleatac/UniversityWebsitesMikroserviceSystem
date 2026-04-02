using MassTransit.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mikroservice.Shared.OpenTelemetry;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;


namespace Microservice.Shared.Extentions
{
    public static class OpenTelemetryTraceExt
    {
        public static IServiceCollection AddOpenTelemetryTraceExt(this IServiceCollection services, IConfiguration configuration)
        {


            //Opentelemetry trace için eklendi.
            var OpenTelemetryConstants = configuration.GetSection("OpenTelemetry").Get<OpenTelemetryConstants>();

            services.AddOpenTelemetry()
                .WithTracing(tracingBuilder => {
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

                        httpOptions.FilterHttpRequestMessage = request => {
                            var pathValue = request.RequestUri?.AbsolutePath;
                            // Null veya boş path kontrolü
                            if (string.IsNullOrEmpty(pathValue))
                                return false;
                            // Hangfire dashboard isteklerini hariç tut
                            //if (pathValue.StartsWith("/hangfire", StringComparison.OrdinalIgnoreCase))
                            //    return false;
                            // '9200' içeren istekleri hariç tut. çünkü elastic earch 9200 portunu kullanır ve bu istekler çok fazla olabilir ve trace'i gereksiz
                            return !pathValue.Contains("9200", StringComparison.InvariantCulture);
                        };


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
                    .AddRedisInstrumentation(options => {
                        options.SetVerboseDatabaseStatements = true; //şimdilik false . true yaparsan redis için trace bilgilerini alırsın.
                    })//Redis
                    .SetSampler(new AlwaysOnSampler())
                    .AddConsoleExporter()
                    .AddOtlpExporter();//jaeger
                })
                .WithMetrics(options => {
                    options.ConfigureResource(metricResource => {

                        metricResource.AddService(OpenTelemetryConstants!.ServiceName, serviceVersion: OpenTelemetryConstants.ServiceVersion);

                    })
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddPrometheusExporter()
                    .AddMeter("university.microservice.api");
                });




            var activitySource = new ActivitySource(
                OpenTelemetryConstants!.ActivitySourceName,
                OpenTelemetryConstants.ServiceVersion
            );



            return services;
        }
    }
}
