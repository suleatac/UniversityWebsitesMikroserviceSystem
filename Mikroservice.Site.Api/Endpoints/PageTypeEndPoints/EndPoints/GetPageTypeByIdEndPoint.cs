using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.PageTypeFeatures.GetPageTypeById;

namespace Mikroservice.Site.Api.Endpoints.PageTypeEndPoints.EndPoints
{
    public static class GetPageTypeByIdEndPoint
    {
        public static RouteGroupBuilder GetPageTypeByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
                (await mediator.Send(new GetPageTypeByIdQuery(id))).ToGenericResult())
                .WithName("GetPageTypeById")
                .MapToApiVersion(1.0)
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            return group;
        }
    }
}