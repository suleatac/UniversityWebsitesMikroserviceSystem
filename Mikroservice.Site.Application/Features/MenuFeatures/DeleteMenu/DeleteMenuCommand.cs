using Microservice.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.MenuFeatures.DeleteMenu
{
    public record DeleteMenuCommand(int Id) : IRequestByServiceResult;
}
