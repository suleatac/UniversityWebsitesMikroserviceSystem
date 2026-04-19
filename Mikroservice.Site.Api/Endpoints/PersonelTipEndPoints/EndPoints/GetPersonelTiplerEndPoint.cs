using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.PersonelTipFeatures.GetPersonelTip;

namespace Mikroservice.Site.Api.Endpoints.PersonelTipEndPoints.EndPoints
{
    public static class GetPersonelTiplerEndPoint
    {
        public static RouteGroupBuilder GetPersonelTiplerEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetPersonelTipQuery());
                return result.ToGenericResult();
            })
            .WithName("GetPersonelTipler")
            .MapToApiVersion(1.0)
            .Produces<List<PersonelTip>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
