using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Microservice.Shared.SeriLog
{
    public static class ExceptionMiddleware
    {

        public static void UseExceptionMiddleware(this WebApplication app)
        {

            app.UseExceptionHandler(config => {

                config.Run(async context => {

                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    var response = ServiceResult<string>.Error(exceptionFeature!.Error.Message,HttpStatusCode.InternalServerError);

                    await context.Response.WriteAsJsonAsync(response);

                });
            });
        }
    }
}
