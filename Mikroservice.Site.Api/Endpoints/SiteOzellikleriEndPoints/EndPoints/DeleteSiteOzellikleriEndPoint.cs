using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.DeleteSiteOzellikleri;

namespace Mikroservice.Site.Api.Endpoints.SiteOzellikleriEndPoints.EndPoints
{
    public static class DeleteSiteOzellikleriEndPoint
    {
        public static RouteGroupBuilder DeleteSiteOzellikleriEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new DeleteSiteOzellikleriCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteSiteOzellikleri")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
