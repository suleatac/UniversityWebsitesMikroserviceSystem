using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.SiteFeatures.DeleteSite;

namespace Mikroservice.Site.Api.Endpoints.SiteEndPoints.EndPoints
{
    public static class DeleteSiteEndPoint
    {
        public static RouteGroupBuilder DeleteSiteEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteSiteCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteSite")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
