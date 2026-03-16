using MediatR;
using Microservice.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Personel.Application.Features.PersonelFeatures.GetPersonel
{
    public record GetPersonelsQuery : IRequestByServiceResult<List<Domain.Entities.Personel>>;
}
