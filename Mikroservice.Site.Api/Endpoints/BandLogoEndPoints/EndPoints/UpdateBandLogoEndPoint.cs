using MediatR;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.BandLogoFeatures.UpdateBandLogo;

namespace Mikroservice.Site.Api.Endpoints.BandLogoEndPoints.EndPoints
{
    public static class UpdateBandLogoEndPoint
    {
            public static RouteGroupBuilder UpdateBandLogoEndpointGroupItem(this RouteGroupBuilder group)
            {
                group.MapPut("/{id}", async (
                    int id,
                    [FromServices] IMediator mediator,
                    [FromBody] UpdateBandLogoCommand command) =>
                {
                    if (id != command.Id)
                        return Results.BadRequest("Id uyuşmuyor");

                    var result = await mediator.Send(command);
                    return result.ToGenericResult();
                })
                .WithName("UpdateBandLogo")
                .MapToApiVersion(1.0)
                .AddEndpointFilter<ValidationFilter<UpdateBandLogoCommand>>()
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);

                return group;
            }
        
    }
}

