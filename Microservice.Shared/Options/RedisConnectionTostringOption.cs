using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Shared.Options
{
    public class RedisConnectionTostringOption
    {
        public const string Key = "ConnectionStrings";
        public string Redis { get; set; } = default!;
    }
}
