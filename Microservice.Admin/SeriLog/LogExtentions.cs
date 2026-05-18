using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Microservice.Admin.SeriLog
{
    public static class LogExtensions
    {
        public static IServiceCollection AddLoggingExt(this IServiceCollection services, IConfiguration configuration)
        {
            //Opentelemetry trace için eklendi.
            var openTelemetryConstants = configuration.GetSection("OpenTelemetry").Get<OpenTelemetryConstants>();

            services.AddOpenTelemetry()
                .WithTracing(tracingBuilder => {
                    tracingBuilder
                    .AddSource(openTelemetryConstants!.ActivitySourceName)
                    .ConfigureResource(resource => {
                        resource.AddService(openTelemetryConstants.ServiceName, serviceVersion: openTelemetryConstants.ServiceVersion);
                    })
                    .AddAspNetCoreInstrumentation(options => {
                      

                        options.RecordException = true;
                    })
                    .AddHttpClientInstrumentation(httpOptions => {

                        httpOptions.FilterHttpRequestMessage = request => {
                            var pathValue = request.RequestUri?.AbsolutePath;
                            // Null veya boş path kontrolü
                            if (string.IsNullOrEmpty(pathValue))
                                return false;

                            // '9200' içeren istekleri hariç tut (Elasticsearch)
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
                        options.SetVerboseDatabaseStatements = true;
                    })
                    .SetSampler(new AlwaysOnSampler())
                    .AddOtlpExporter();//Jaeger
                });

            return services;
        }

    }
}
