using MediatR;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.BilgiFeatures.CreateBilgi;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.BilgiEndPoints.EndPoints
{
    public static class CreateBilgiEndPoint
    {
        public static RouteGroupBuilder CreateBilgiEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateBilgiCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateBilgi")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateBilgiCommand>>();

            return group;
        }
    }
}
