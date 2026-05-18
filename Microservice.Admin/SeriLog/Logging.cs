using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;

namespace Microservice.Admin.SeriLog
{
    public class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogging => (context, configuration) =>
        {
            var environment= context.HostingEnvironment;
            configuration
                .ReadFrom.Configuration(context.Configuration)//appsettingsten alıyo burda ayarları.
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("Environment", environment.EnvironmentName)
                .Enrich.WithProperty("ApplicationName", environment.ApplicationName);

            var eleasticSearchUrl = context.Configuration.GetSection("ElasticSearch")["BaseUrl"];
            var userName = context.Configuration.GetSection("ElasticSearch")["UserName"];
            var password = context.Configuration.GetSection("ElasticSearch")["Password"];
            var indexName = context.Configuration.GetSection("ElasticSearch")["IndexName"];

            configuration.WriteTo.Elasticsearch(new(new Uri(eleasticSearchUrl!)) {

                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion=Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv8,
                    IndexFormat= $"{indexName}-{environment.EnvironmentName.ToLower()}-logs-{DateTime.UtcNow:yyyy.MM.dd}",
                    ModifyConnectionSettings = x => x.BasicAuthentication(userName, password),
                    CustomFormatter= new ElasticsearchJsonFormatter()
            });

            

        };
    }
}
