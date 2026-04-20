using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.DeleteSikcaSorulanSoru;

namespace Mikroservice.Site.Api.Endpoints.SikcaSorulanSoruEndPoints.EndPoints
{
    public static class DeleteSikcaSorulanSoruEndPoint
    {
        public static RouteGroupBuilder DeleteSikcaSorulanSoruEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteSikcaSorulanSoruCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteSikcaSorulanSoru")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
