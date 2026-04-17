using Microservice.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.DeleteEtkinlik
{
    public record DeleteEtkinlikCommand(int Id) : IRequestByServiceResult;
}
