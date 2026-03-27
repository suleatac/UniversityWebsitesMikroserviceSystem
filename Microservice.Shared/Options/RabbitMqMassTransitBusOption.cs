using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Shared.Options
{
    public class RabbitMqMassTransitBusOption
    {
        public required string Address { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Port { get; set; }
    }
}
