using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.PageBlockFeatures.CreatePageBlock;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.PageSectionEndPoints.EndPoints
{
    public static class CreatePageBlockEndPoint
    {
        public static RouteGroupBuilder CreatePageBlockEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/{sectionId:int}/blocks", async (
                int sectionId,
                [FromServices] IMediator mediator,
                [FromBody] CreatePageBlockCommand command) =>
            {
                if (sectionId != command.PageSectionId)
                    return Results.BadRequest("sectionId ile command.PageSectionId uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreatePageBlock")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreatePageBlockCommand>>();

            return group;
        }
    }
}
