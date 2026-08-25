using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.PageTypeFeatures.GetPageTypes;

namespace Mikroservice.Site.Api.Endpoints.PageTypeEndPoints.EndPoints
{
    public static class GetPageTypesEndPoint
    {
        public static RouteGroupBuilder GetPageTypesEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) =>
                (await mediator.Send(new GetPageTypesQuery())).ToGenericResult())
                .WithName("GetPageTypes")
                .MapToApiVersion(1.0)
                .Produces(StatusCodes.Status200OK);

            return group;
        }
    }
}