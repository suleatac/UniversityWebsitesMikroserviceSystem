using MediatR;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.DeleteSertifikaParmakIzi;

namespace Mikroservice.Site.Api.Endpoints.SertifikaParmakIziEndPoints.EndPoints
{
    public static class DeleteSertifikaParmakIziEndPoint
    {
        public static RouteGroupBuilder DeleteSertifikaParmakIziEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapDelete("/{id:int}", async (IMediator mediator, int id) =>
            {
                if (id <= 0)
                    return Results.BadRequest("Geçersiz Id");

                var result = await mediator.Send(new DeleteSertifikaParmakIziCommand(id));
                return result.ToGenericResult();
            })
            .WithName("DeleteSertifikaParmakIzi")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
