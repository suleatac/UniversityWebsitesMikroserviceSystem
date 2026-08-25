using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.PageTypeFeatures.CreatePageType;

namespace Mikroservice.Site.Api.Endpoints.PageTypeEndPoints.EndPoints
{
    public static class CreatePageTypeEndPoint
    {
        public static RouteGroupBuilder CreatePageTypeEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async ([FromServices] IMediator mediator, [FromBody] CreatePageTypeCommand command) =>
                (await mediator.Send(command)).ToGenericResult())
                .WithName("CreatePageType")
                .MapToApiVersion(1.0)
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .AddEndpointFilter<ValidationFilter<CreatePageTypeCommand>>();

            return group;
        }
    }
}