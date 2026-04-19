using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Microservice.Site.Application.Features.YonetimDuyuruFeatures.UpdateYonetimDuyuru;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Site.Api.Endpoints.YonetimDuyuruEndPoints.EndPoints
{
    public static class UpdateYonetimDuyuruEndPoint
    {
        public static RouteGroupBuilder UpdateYonetimDuyuruEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id}", async (
                  int id,
                  [FromServices] IMediator mediator,
                  [FromBody] UpdateYonetimDuyuruCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
              .WithName("UpdateYonetimDuyuru")
                .MapToApiVersion(1.0)
                .AddEndpointFilter<ValidationFilter<UpdateYonetimDuyuruCommand>>()
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
