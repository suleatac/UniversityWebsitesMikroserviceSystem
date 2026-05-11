using MediatR;
using Mikroservice.Site.Application.DTOs.DuyuruDtos;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.DuyuruFeatures.GetDuyuruById;

namespace Mikroservice.Site.Api.Endpoints.DuyuruEndPoints.EndPoints
{
    public static class GetDuyuruByIdEndPoint
    {
        public static RouteGroupBuilder GetDuyuruByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetDuyuruByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetDuyuruById")
            .MapToApiVersion(1.0)
            .Produces<DuyuruDetailDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}