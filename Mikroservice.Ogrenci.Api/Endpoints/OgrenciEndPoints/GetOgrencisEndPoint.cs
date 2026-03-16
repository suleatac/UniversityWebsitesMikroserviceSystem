using MediatR;
using Microservice.Ogrenci.Application.Features.OgrenciFeatures.GetOgrenci;

namespace Mikroservice.Ogrenci.Api.Endpoints.OgrenciEndPoints
{
    public static class GetOgrencisEndPoint
    {
        public static RouteGroupBuilder GetOgrencisGroupItemEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) => {
                var result = await mediator.Send(new GetOgrencisQuery());
                return result;
            })
             .WithName("GetOgrencis")
             .Produces<Guid>(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
