using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.IletisimFeatures.GonderIletisimMesaji;

namespace Mikroservice.Site.Api.Endpoints.IletisimEndPoints.EndPoints
{
    public static class GonderIletisimMesajiEndPoint
    {
        public static RouteGroupBuilder GonderIletisimMesajiEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                IMediator mediator,
                GonderIletisimMesajiCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("GonderIletisimMesaji")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<GonderIletisimMesajiCommand>>();

            return group;
        }
    }
}
