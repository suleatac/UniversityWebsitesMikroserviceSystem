using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.DilFeatures.GetDil;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Api.Endpoints.DilEndPoints.EndPoints
{
    public static class GetDilsEndPoint
    {
        public static RouteGroupBuilder GetDilsEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetDilQuery());
                return result.ToGenericResult();
            })
            .WithName("GetDils")
            .MapToApiVersion(1.0)
            .Produces<List<Dil>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
