using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Microservice.Admin.Services.ServiceResults
{
    public class ServiceResult
    {

        public ProblemDetails? Fail { get; set; }
        [JsonIgnore] public bool IsSuccess => Fail is null;
        [JsonIgnore] public bool IsFail => !IsSuccess;

        // Static factory method for success result with 200 OK status
        public static ServiceResult Success()
        {

            return new ServiceResult();

        }




        public static ServiceResult Error(ProblemDetails? problemdetails)
        {

            return new ServiceResult
            {
                Fail = problemdetails
            };

        }
        public static ServiceResult Error(string title, string description)
        {

            return new ServiceResult
            {

                Fail = new ProblemDetails()
                {
                    Title = title,
                    Detail = description
                }
            };

        }
        public static ServiceResult Error(string title)
        {

            return new ServiceResult
            {
                Fail = new ProblemDetails()
                {
                    Title = title
                }
            };

        }




    }
    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; } = default!;
        // Static factory method for success result with 200 OK status
        public static ServiceResult<T> Success(T data)
        {

            return new ServiceResult<T>
            {
                Data = data
            };

        }



        public new static ServiceResult<T> Error(ProblemDetails? problemdetails)
        {

            return new ServiceResult<T>
            {
                Fail = problemdetails
            };

        }
        public new static ServiceResult<T> Error(string title, string description)
        {

            return new ServiceResult<T>
            {
                Fail = new ProblemDetails()
                {
                    Title = title,
                    Detail = description
                }
            };

        }
        public new static ServiceResult<T> Error(string title)
        {

            return new ServiceResult<T>
            {
                Fail = new ProblemDetails()
                {
                    Title = title
                }
            };

        }

    }
}
