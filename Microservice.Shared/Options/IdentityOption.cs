using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Shared.Options
{
    public class IdentityOption
    {
        public required string Adress { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
    }
}
