using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.GaleriResimDtos;
using Mikroservice.Site.Application.Features.GaleriResimFeatures.GetGaleriResimById;

namespace Mikroservice.Site.Api.Endpoints.GaleriResimEndPoints.EndPoints
{
    public static class GetGaleriResimByIdEndPoint
    {
        public static RouteGroupBuilder GetGaleriResimByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetGaleriResimByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetGaleriResimById")
            .MapToApiVersion(1.0)
            .Produces<GaleriResimDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
