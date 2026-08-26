using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.DilFeatures.GetDilById;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Api.Endpoints.DilEndPoints.EndPoints
{
    public static class GetDilByIdEndPoint
    {
        public static RouteGroupBuilder GetDilByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetDilByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetDilById")
            .MapToApiVersion(1.0)
            .Produces<Dil>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
