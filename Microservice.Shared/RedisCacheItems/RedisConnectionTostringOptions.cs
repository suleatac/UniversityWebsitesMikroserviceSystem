using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Shared.RedisCacheItems
{
    public class RedisConnectionTostringOptions
    {
        public const string Key = "ConnectionStrings";
        public string Redis { get; set; } = default!;
    }
}
