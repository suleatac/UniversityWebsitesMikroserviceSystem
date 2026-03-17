using MediatR;
using Microservice.Personel.Application.Features.PersonelFeatures.GetPersonel;
using Microservice.Shared.Extentions;

namespace Mikroservice.Personel.Api.Endpoints.Personels
{
    public static class GetPersonelsEndPoint
    {
        public static RouteGroupBuilder GetPersonelsGroupItemEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) => {
                var result = await mediator.Send(new GetPersonelsQuery());
                return result.ToGenericResult();
            })
             .WithName("GetPersonels")
             .MapToApiVersion(1.0)
             .Produces<Guid>(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
