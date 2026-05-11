using MediatR;
using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.GetSikcaSorulanSoruById;

namespace Mikroservice.Site.Api.Endpoints.SikcaSorulanSoruEndPoints.EndPoints
{
    public static class GetSikcaSorulanSoruByIdEndPoint
    {
        public static RouteGroupBuilder GetSikcaSorulanSoruByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetSikcaSorulanSoruByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetSikcaSorulanSoruById")
            .MapToApiVersion(1.0)
            .Produces<SikcaSorulanSoru>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}