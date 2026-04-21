using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.MediaFileFeatures.CreateMediaFile;

namespace Mikroservice.Site.Api.Endpoints.MediaFileEndPoints.EndPoints
{
    public static class CreateMediaFileEndPoint
    {
        public static RouteGroupBuilder CreateMediaFileEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateMediaFileCommand command) => {
                    var result = await mediator.Send(command);
                    return result.ToGenericResult();
                })
            .WithName("CreateMediaFile")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateMediaFileCommand>>();

            return group;
        }
    }
}
