using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.PageSectionFeatures.CreatePageSection;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.PageSectionEndPoints.EndPoints
{
    public static class CreatePageSectionEndPoint
    {
        public static RouteGroupBuilder CreatePageSectionEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreatePageSectionCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreatePageSection")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreatePageSectionCommand>>();

            return group;
        }
    }
}
