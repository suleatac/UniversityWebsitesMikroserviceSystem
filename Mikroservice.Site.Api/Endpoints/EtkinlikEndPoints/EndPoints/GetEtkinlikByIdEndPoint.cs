using MediatR;
using Mikroservice.Site.Application.DTOs.EtkinlikDtos;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.EtkinlikFeatures.GetEtkinlikById;

namespace Mikroservice.Site.Api.Endpoints.EtkinlikEndPoints.EndPoints
{
    public static class GetEtkinlikByIdEndPoint
    {
        public static RouteGroupBuilder GetEtkinlikByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetEtkinlikByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetEtkinlikById")
            .MapToApiVersion(1.0)
            .Produces<EtkinlikDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}