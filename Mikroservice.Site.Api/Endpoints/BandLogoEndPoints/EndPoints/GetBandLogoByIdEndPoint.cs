using MediatR;
using Mikroservice.Site.Application.DTOs.BandLogoDtos;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.BandLogoFeatures.GetBandLogoById;

namespace Mikroservice.Site.Api.Endpoints.BandLogoEndPoints.EndPoints
{
    public static class GetBandLogoByIdEndPoint
    {
        public static RouteGroupBuilder GetBandLogoByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetBandLogoByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetBandLogoById")
            .MapToApiVersion(1.0)
            .Produces<BandLogoDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
