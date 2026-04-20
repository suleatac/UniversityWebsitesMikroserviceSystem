using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.SitePersonelFeatures.DeleteSitePersonel;

namespace Mikroservice.Site.Api.Endpoints.SitePersonelEndPoints.EndPoints
{
    public static class DeleteSitePersonelEndPoint
    {
        public static RouteGroupBuilder DeleteSitePersonelEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteSitePersonelCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteSitePersonel")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
