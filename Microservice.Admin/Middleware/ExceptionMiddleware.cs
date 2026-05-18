using Microsoft.AspNetCore.Diagnostics;

namespace Microservice.Admin.Middleware
{
    public static class ExceptionMiddleware
    {
        public static void UseExceptionMiddleware(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var feature =
                        context.Features.Get<IExceptionHandlerFeature>();

                    var logger =
                        context.RequestServices.GetRequiredService<ILogger<Program>>();

                    var traceId =
                        context.Items["TraceId"]?.ToString()
                        ?? context.TraceIdentifier;

                    if (feature != null)
                    {
                        logger.LogError(
                            feature.Error,
                            "Unhandled exception | TraceId: {TraceId}",
                            traceId);
                    }

                    context.Response.Redirect($"/Error?traceId={traceId}");
                });
            });
        }
    }
}
