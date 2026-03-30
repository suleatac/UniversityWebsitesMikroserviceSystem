using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Shared.OpenTelemetry
{
    public class Counter
    {
        public static int OrderCancelledCounter { get; set; }

        public static int CurrentStockCount { get; set; } = 1000;

        public static int KitchenTemp { get; set; } = 0;
    }
}
