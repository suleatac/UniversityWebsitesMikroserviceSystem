using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.GetSertifikaParmakIzi;

namespace Mikroservice.Site.Api.Endpoints.SertifikaParmakIziEndPoints.EndPoints
{
    public static class GetSertifikaParmakIzleriEndPoint
    {
        public static RouteGroupBuilder GetSertifikaParmakIzleriEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetSertifikaParmakIziQuery());
                return result.ToGenericResult();
            })
            .WithName("GetSertifikaParmakIzleri")
            .MapToApiVersion(1.0)
            .Produces<List<SertifikaParmakIzi>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
