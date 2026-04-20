using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.UnvanFeatures.GetUnvans;

namespace Mikroservice.Site.Api.Endpoints.UnvanEndPoints.EndPoints
{
    public static class GetUnvanlarEndPoint
    {
        public static RouteGroupBuilder GetUnvanlarEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                IMediator mediator) =>
            {
                var result = await mediator.Send(new GetUnvansQuery());
                return result.ToGenericResult();
            })
            .WithName("GetUnvanlar")
            .MapToApiVersion(1.0)
            .Produces<List<Unvan>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
