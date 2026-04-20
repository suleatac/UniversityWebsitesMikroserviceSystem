using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.DeleteSikcaSorulanSoruKategori;

namespace Mikroservice.Site.Api.Endpoints.SikcaSorulanSoruKategoriEndPoints.EndPoints
{
    public static class DeleteSikcaSorulanSoruKategoriEndPoint
    {
        public static RouteGroupBuilder DeleteSikcaSorulanSoruKategoriEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteSikcaSorulanSoruKategoriCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteSikcaSorulanSoruKategori")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
