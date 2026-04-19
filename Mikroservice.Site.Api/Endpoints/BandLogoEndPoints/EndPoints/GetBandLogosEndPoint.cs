using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.BandLogoFeatures.GetBandLogos;
using Mikroservice.Site.Domain.Entities;
namespace Mikroservice.Site.Api.Endpoints.BandLogoEndPoints.EndPoints
{
    public static class GetBandLogosEndPoint
    {
        public static RouteGroupBuilder GetBandLogosEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (int siteId, int dilId, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetBandLogosQuery(siteId, dilId));
                return result.ToGenericResult();
            })
            .WithName("GetBandLogos")
            .MapToApiVersion(1.0)
            .Produces<List<BandLogo>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
