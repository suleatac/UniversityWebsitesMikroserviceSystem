using Microsoft.AspNetCore.Http;
using System.Net;

namespace Microservice.Shared.Extentions
{
    public static class EndpointResultExt
    {
        public static IResult ToGenericResult<T>(this ServiceResult<T> result)
        {
            return result.StatusCode switch {
                HttpStatusCode.OK => Results.Ok(result.Data),

                HttpStatusCode.Created => Results.Created(
                    result.UrlAsCreated ?? string.Empty,
                    result.Data
                ),

                HttpStatusCode.BadRequest => Results.BadRequest(result.Fail),

                HttpStatusCode.NotFound => Results.NotFound(result.Fail),

                HttpStatusCode.NoContent => Results.NoContent(),

                _ => Results.Problem(
                    title: result.Fail?.Title,
                    detail: result.Fail?.Detail
                )
            };
        }

        public static IResult ToGenericResult(this ServiceResult result)
        {
            return result.StatusCode switch {
                HttpStatusCode.OK => Results.Ok(result),

                HttpStatusCode.NoContent => Results.NoContent(),

                HttpStatusCode.BadRequest => Results.BadRequest(result.Fail),

                HttpStatusCode.NotFound => Results.NotFound(result.Fail),

                _ => Results.Problem(
                    title: result.Fail?.Title,
                    detail: result.Fail?.Detail
                )
            };
        }
    }
}