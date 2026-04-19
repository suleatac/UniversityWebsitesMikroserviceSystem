using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.PersonelTipFeatures.DeletePersonelTip;

namespace Mikroservice.Site.Api.Endpoints.PersonelTipEndPoints.EndPoints
{
    public static class DeletePersonelTipEndPoint
    {
        public static RouteGroupBuilder DeletePersonelTipEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeletePersonelTipCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeletePersonelTip")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
