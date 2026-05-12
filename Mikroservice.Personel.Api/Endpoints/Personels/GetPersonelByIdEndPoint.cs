using MediatR;
using Microservice.Personel.Application.Features.PersonelFeatures.GetPersonelById;
using Microservice.Shared.Extentions;

namespace Mikroservice.Personel.Api.Endpoints.Personels
{
    public static class GetPersonelByIdEndPoint
    {
        public static RouteGroupBuilder GetPersonelByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id}", async (IMediator mediator, int id) => {
                var result = await mediator.Send(new GetPersonelByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetPersonelById")
            .MapToApiVersion(1.0)
            .Produces<Microservice.Personel.Domain.Entities.Personel>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
