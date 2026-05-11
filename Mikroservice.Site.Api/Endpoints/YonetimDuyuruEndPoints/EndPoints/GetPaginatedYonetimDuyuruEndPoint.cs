using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.YonetimDuyuru;
using Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.GetPaginatedYonetimDuyuru;

namespace Mikroservice.Site.Api.Endpoints.YonetimDuyuruEndPoints.EndPoints
{
    public static class GetPaginatedYonetimDuyuruEndPoint
    {
        
    
        public static RouteGroupBuilder GetPaginatedYonetimDuyuruEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/paginated", async 
                (
                IMediator mediator,
                [AsParameters] GetPaginatedYonetimDuyuruQuery query
                ) => 
                   {
                      var result = await mediator.Send(query);
                       return result.ToGenericResult();
                   }
                )
            .WithName("GetPaginatedYonetimDuyuru")
            .MapToApiVersion(1.0)
            .Produces<PaginatedResult<YonetimDuyuruDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}

