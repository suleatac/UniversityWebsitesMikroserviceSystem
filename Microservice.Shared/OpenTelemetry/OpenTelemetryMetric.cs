using System.Diagnostics.Metrics;

namespace Microservice.Shared.OpenTelemetry
{
    public static class OpenTelemetryMetric
    {
        // Uygulama adı veya mikroservis adı ile aynı olmalı
        private static readonly Meter meter = new Meter("university.microservice.api");

        // Personel API için örnek metricler
        public static Counter<int> PersonelGetPersonelsQueryExecuted = meter.CreateCounter<int>("personel.fetched.count");
        public static Histogram<long> PersonelFetchDuration = meter.CreateHistogram<long>("personel.fetch.duration", unit: "milliseconds");

        // Öğrenci API için örnek metricler
        public static Counter<int> OgrenciGetOgrencisQueryExecuted = meter.CreateCounter<int>("ogrenci.fetched.count");
        public static Histogram<long> OgrenciFetchDuration = meter.CreateHistogram<long>("ogrenci.fetch.duration", unit: "milliseconds");

        // Ortak kullanılabilecek örnek metricler
        public static Counter<int> ApiErrorCounter = meter.CreateCounter<int>("api.error.count");
        public static Histogram<long> ApiRequestDuration = meter.CreateHistogram<long>("api.request.duration", unit: "milliseconds");
    }
}

//// Personel verisi çekildiğinde:
//OpenTelemetryMetric.PersonelFetchedCounter.Add(1);

//// Öğrenci verisi çekildiğinde:
//OpenTelemetryMetric.OgrenciFetchedCounter.Add(1);

//// API isteği süresi ölçümü:
//using var activity = Activity.Current;
//var stopwatch = Stopwatch.StartNew();
//// ...işlem...
//stopwatch.Stop();
//OpenTelemetryMetric.ApiRequestDuration.Record(stopwatch.ElapsedMilliseconds);