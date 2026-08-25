using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Microservice.Site.Application.Features.PageTypeFeatures.UpdatePageType;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.PageTypeEndPoints.EndPoints
{
    public static class UpdatePageTypeEndPoint
    {
        public static RouteGroupBuilder UpdatePageTypeEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (int id, [FromServices] IMediator mediator, [FromBody] UpdatePageTypeCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                return (await mediator.Send(command)).ToGenericResult();
            })
            .WithName("UpdatePageType")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .AddEndpointFilter<ValidationFilter<UpdatePageTypeCommand>>();

            return group;
        }
    }
}