using MediatR;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.TemplateFeatures.UpdateTemplate;

namespace Mikroservice.Site.Api.Endpoints.TemplateEndPoints.EndPoints
{
    public static class UpdateTemplateEndPoint
    {
        public static RouteGroupBuilder UpdateTemplateEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (
                int id,
                [FromServices] IMediator mediator,
                [FromBody] UpdateTemplateCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("UpdateTemplate")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateTemplateCommand>>();

            return group;
        }
    }
}