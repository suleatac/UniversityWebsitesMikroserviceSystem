
using MediatR;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.BannerFeatures.UpdateBanner;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.BannerEndPoints.EndPoints
{
    public static class UpdateBannerEndPoint
    {
        public static RouteGroupBuilder UpdateBannerEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/{id:int}", async (
                int id,
                [FromServices] IMediator mediator,
                [FromBody] UpdateBannerCommand command) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("Id uyuşmuyor");

                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("UpdateBanner")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<UpdateBannerCommand>>();

            return group;
        }
    }
}
