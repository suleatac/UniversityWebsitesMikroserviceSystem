using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.BandLogoFeatures.DeleteBandLogo;

namespace Mikroservice.Site.Api.Endpoints.BandLogoEndPoints.EndPoints
{
    public static class DeleteBandLogoEndPoint
    {
        public static RouteGroupBuilder DeleteBandLogoEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) => {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteBandLogoCommand(id));
                return result.ToGenericResult();
            })
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("DeleteBandLogo")
            .WithSummary("Delete Band Logo")
            .WithDescription("Delete Band Logo by Id");

            return group;
        }
    }
}
