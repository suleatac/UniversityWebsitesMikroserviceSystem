using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.PageSectionDtos;
using Mikroservice.Site.Application.Features.PageSectionFeatures.GetPageSections;

namespace Mikroservice.Site.Api.Endpoints.PageSectionEndPoints.EndPoints
{
    public static class GetPageSectionsEndPoint
    {
        public static RouteGroupBuilder GetPageSectionsEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (
                int siteId,
                int dilId,
                bool publishedOnly,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new GetPageSectionsQuery(siteId, dilId, publishedOnly));
                return result.ToGenericResult();
            })
            .WithName("GetPageSections")
            .MapToApiVersion(1.0)
            .Produces<List<PageSectionDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
