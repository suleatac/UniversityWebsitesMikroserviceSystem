using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.IcerikDtos;
using Mikroservice.Site.Application.Features.IcerikFeatures.SearchIcerik;

namespace Mikroservice.Site.Api.Endpoints.IcerikEndPoints.EndPoints
{
    public static class SearchIcerikEndPoint
    {
        public static RouteGroupBuilder SearchIcerikEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/search", async (IMediator mediator,
              [AsParameters] SearchIcerikQuery query) => {
              var result = await mediator.Send(query);
              return result.ToGenericResult();
          })
              .WithName("SearchIcerik")
              .MapToApiVersion(1.0)
              .Produces<PaginatedResult<IcerikSearchDto>>(StatusCodes.Status200OK)
              .Produces(StatusCodes.Status400BadRequest)
              .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
