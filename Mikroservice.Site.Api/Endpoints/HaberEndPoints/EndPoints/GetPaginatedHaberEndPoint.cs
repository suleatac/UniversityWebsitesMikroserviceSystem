using MediatR;
using Mikroservice.Site.Application.DTOs;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.HaberDtos;
using Mikroservice.Site.Application.Features.HaberFeatures.GetPaginatedHaber;

namespace Mikroservice.Site.Api.Endpoints.HaberEndPoints.EndPoints
{
    public static class GetPaginatedHaberEndPoint
    {
        public static RouteGroupBuilder GetPaginatedHaberEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/paginated", async (IMediator mediator,
              [AsParameters] GetPaginatedHaberQuery query) => {
              var result = await mediator.Send(query);
              return result.ToGenericResult();
      })
              .WithName("GetPaginatedHaber")
            .MapToApiVersion(1.0)
            .Produces<PaginatedResult<HaberDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
