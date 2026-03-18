using MediatR;
using Refit;
using System.Net;
using System.Text.Json.Serialization;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Microservice.Shared
{
    public interface IRequestByServiceResult<T> : IRequest<ServiceResult<T>>;
    public interface IRequestByServiceResult : IRequest<ServiceResult>;



    public class ServiceResult
    {
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }
        public ProblemDetails? Fail { get; set; }
        [JsonIgnore] public bool IsSuccess => Fail is null;
        [JsonIgnore] public bool IsFail => !IsSuccess;

        // Static factory method for success result with 200 OK status
        public static ServiceResult SuccessAsNoContent()
        {

            return new ServiceResult {
                StatusCode = HttpStatusCode.NoContent
            };

        }
        // Static factory method for error result with 404 Not Found status
        public static ServiceResult ErrorAsNotFound()
        {

            return new ServiceResult {
                StatusCode = HttpStatusCode.NotFound,
                Fail = new ProblemDetails {
                    Status = (int)HttpStatusCode.NotFound,
                    Title = "Not Found",
                    Detail = "The requested resource was not found."
                }
            };

        }



        public static ServiceResult ErrorFromProblemDetails(ApiException exception)
        {
            if (string.IsNullOrEmpty(exception.Content))
            {
                return new ServiceResult() {

                    Fail = new ProblemDetails() {

                        Title = "Not Found"

                    },
                    StatusCode = exception.StatusCode
                };
            }

            var problemDetails = System.Text.Json.JsonSerializer.Deserialize<ProblemDetails>(exception.Content, new System.Text.Json.JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            });


            return new ServiceResult() {
                Fail = problemDetails,
                StatusCode = exception.StatusCode
            };







        }

        public static ServiceResult Error(ProblemDetails? problemdetails, HttpStatusCode status)
        {

            return new ServiceResult {
                StatusCode = status,
                Fail = problemdetails
            };

        }
        public static ServiceResult Error(string title, string description, HttpStatusCode status)
        {

            return new ServiceResult {
                StatusCode = status,
                Fail = new ProblemDetails() {
                    Title = title,
                    Detail = description,
                    Status = (int)status
                }
            };

        }
        public static ServiceResult Error(string title, HttpStatusCode status)
        {

            return new ServiceResult {
                StatusCode = status,
                Fail = new ProblemDetails() {
                    Title = title,
                    Status = (int)status
                }
            };

        }
        public static ServiceResult ErrorFromValidation(IDictionary<string, object?> errors)
        {

            return new ServiceResult {
                StatusCode = HttpStatusCode.BadRequest,
                Fail = new ProblemDetails() {
                    Title = "Validation errors occured",
                    Detail = "Please check the errors property for more details",
                    Status = HttpStatusCode.BadRequest.GetHashCode(),
                    Extensions = errors
                }
            };

        }



    }
    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; } = default!;
        [JsonIgnore] public string? UrlAsCreated { get; set; }
        // Static factory method for success result with 200 OK status
        public static ServiceResult<T> SuccessAsOK(T data)
        {

            return new ServiceResult<T> {
                StatusCode = HttpStatusCode.OK,
                Data = data
            };

        }
        // Static factory method for success result with 201 Created status
        public static ServiceResult<T> SuccessAsCreated(T data, string url)
        {

            return new ServiceResult<T> {
                StatusCode = HttpStatusCode.OK,
                Data = data,
                UrlAsCreated = url
            };

        }



        public new static ServiceResult<T> ErrorFromProblemDetails(ApiException exception)
        {
            if (string.IsNullOrEmpty(exception.Content))
            {
                return new ServiceResult<T>() {

                    Fail = new ProblemDetails() {

                        Title = "Not Found"

                    },
                    StatusCode = exception.StatusCode
                };
            }

            var problemDetails = System.Text.Json.JsonSerializer.Deserialize<ProblemDetails>(exception.Content, new System.Text.Json.JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            });


            return new ServiceResult<T>() {
                Fail = problemDetails,
                StatusCode = exception.StatusCode
            };







        }

        public new static ServiceResult<T> Error(ProblemDetails? problemdetails, HttpStatusCode status)
        {

            return new ServiceResult<T> {
                StatusCode = status,
                Fail = problemdetails
            };

        }
        public new static ServiceResult<T> Error(string title, string description, HttpStatusCode status)
        {

            return new ServiceResult<T> {
                StatusCode = status,
                Fail = new ProblemDetails() {
                    Title = title,
                    Detail = description,
                    Status = (int)status
                }
            };

        }
        public new static ServiceResult<T> Error(string title, HttpStatusCode status)
        {

            return new ServiceResult<T> {
                StatusCode = status,
                Fail = new Microsoft.AspNetCore.Mvc.ProblemDetails() {
                    Title = title,
                    Status = (int)status
                }
            };

        }
        public new static ServiceResult<T> ErrorFromValidation(IDictionary<string, object?> errors)
        {

            return new ServiceResult<T> {
                StatusCode = HttpStatusCode.BadRequest,
                Fail = new ProblemDetails() {
                    Title = "Validation errors occured",
                    Detail = "Please check the errors property for more details",
                    Status = HttpStatusCode.BadRequest.GetHashCode(),
                    Extensions = errors
                }
            };

        }
    }
}
