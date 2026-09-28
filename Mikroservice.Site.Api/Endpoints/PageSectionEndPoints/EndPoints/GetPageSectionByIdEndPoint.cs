using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.DTOs.PageSectionDtos;
using Mikroservice.Site.Application.Features.PageSectionFeatures.GetPageSectionById;

namespace Mikroservice.Site.Api.Endpoints.PageSectionEndPoints.EndPoints
{
    public static class GetPageSectionByIdEndPoint
    {
        public static RouteGroupBuilder GetPageSectionByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (
                int id,
                IMediator mediator) =>
            {
                var result = await mediator.Send(new GetPageSectionByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetPageSectionById")
            .MapToApiVersion(1.0)
            .Produces<PageSectionDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
