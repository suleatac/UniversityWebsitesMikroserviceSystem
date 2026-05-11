using MediatR;
using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.PersonelTipFeatures.GetPersonelTipById;

namespace Mikroservice.Site.Api.Endpoints.PersonelTipEndPoints.EndPoints
{
    public static class GetPersonelTipByIdEndPoint
    {
        public static RouteGroupBuilder GetPersonelTipByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetPersonelTipByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetPersonelTipById")
            .MapToApiVersion(1.0)
            .Produces<PersonelTip>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}