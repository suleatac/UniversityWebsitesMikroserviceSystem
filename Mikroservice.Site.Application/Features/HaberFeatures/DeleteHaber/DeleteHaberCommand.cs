using Microservice.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.HaberFeatures.DeleteHaber
{
    public record DeleteHaberCommand(int Id) : IRequestByServiceResult;
}
