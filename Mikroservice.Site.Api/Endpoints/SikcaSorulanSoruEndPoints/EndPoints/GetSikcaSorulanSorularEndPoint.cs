using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.GetSikcaSorulanSoru;

namespace Mikroservice.Site.Api.Endpoints.SikcaSorulanSoruEndPoints.EndPoints
{
    public static class GetSikcaSorulanSorularEndPoint
    {
        public static RouteGroupBuilder GetSikcaSorulanSorularEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                int dilId,
                IMediator mediator) =>
            {
                var result = await mediator.Send(
                    new GetSikcaSorulanSoruQuery(siteId, dilId)
                );

                return result.ToGenericResult();
            })
            .WithName("GetSikcaSorulanSorular")
            .MapToApiVersion(1.0)
            .Produces<List<SikcaSorulanSoru>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
