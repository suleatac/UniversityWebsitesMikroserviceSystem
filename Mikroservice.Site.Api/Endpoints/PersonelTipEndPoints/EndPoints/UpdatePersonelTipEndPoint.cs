using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.PersonelTipFeatures.UpdatePersonelTip;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.PersonelTipEndPoints.EndPoints
{
    public static class UpdatePersonelTipEndPoint
    {
        public static RouteGroupBuilder UpdatePersonelTipEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (
                int id,
                [FromServices] IMediator mediator,
                [FromBody] UpdatePersonelTipCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("UpdatePersonelTip")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdatePersonelTipCommand>>();

            return group;
        }
    }
}
