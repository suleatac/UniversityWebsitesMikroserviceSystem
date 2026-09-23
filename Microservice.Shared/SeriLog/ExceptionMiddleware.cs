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
                    var error = exceptionFeature!.Error;

                    // PostgreSQL 23505 = unique violation. Bu bir veri celismesi; kullanicinin
                    // duzeltebilecegi bir durum oldugu icin 500 degil 409 Conflict donulur
                    // (orn. ayni site'de SeoUrl benzersizlik index'i "IX_Icerik_SiteId_SeoUrl").
                    var isUniqueViolation = IsUniqueViolation(error);

                    var status = isUniqueViolation
                        ? HttpStatusCode.Conflict
                        : HttpStatusCode.InternalServerError;

                    var message = isUniqueViolation
                        ? "Bu kaydin bir alani (muhtemelen SEO adresi) baska bir kayit tarafindan kullaniliyor."
                        : error.Message;

                    var response = ServiceResult<string>.Error(message, status);

                    await context.Response.WriteAsJsonAsync(response);

                });
            });
        }

        // Npgsql referansi olmasa da calissin diye type-name ile kontrol edilir;
        // SqlState property'si reflection ile okunur.
        private static bool IsUniqueViolation(Exception error)
        {
            // PostgresException sinifi assembly'de yoksa (orn. Npgsql'siz servisler) sessizce false doner.
            var exType = error.GetType();
            if (exType.Name != "PostgresException")
                return false;

            var sqlState = exType.GetProperty("SqlState")?.GetValue(error) as string;
            return sqlState == "23505";
        }
    }
}
