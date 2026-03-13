using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Personel.Application.Features.PersonelFeatures.GetPersonel
{
    public record GetPersonelsQuery : IRequest<List<Domain.Entities.Personel>>;
}
