# Prometheus Monitoring Kurulumu

Bu klasör, University Microservices sisteminin Prometheus monitoring yapılandırmasını içerir.

## Prometheus Yapılandırması

### Prometheus'a Erişim
- **URL**: http://localhost:9090
- **Metrics Dashboard**: http://localhost:9090/graph
- **Targets**: http://localhost:9090/targets

## Monitör Edilen Servisler

1. **Prometheus** - Kendisi (localhost:9090)
2. **Oğrenci Mikroservisi** - mikroservice.ogrenci.api:8080/metrics
3. **Personel Mikroservisi** - mikroservice.personel.api:8080/metrics
4. **Gateway** - mikroservice.gateway:8080/metrics
5. **RabbitMQ** - rabbitmq:15692/metrics

## Network Yapılandırması

Tüm servisler `university-network` adlı Docker bridge network üzerinde çalışır. Bu sayede servisler birbirleriyle container adlarını kullanarak iletişim kurabilir.

## .NET API'lerine Metrics Desteği Ekleme

Her bir .NET API projesine aşağıdaki adımları uygulayın:

### 1. NuGet Paketlerini Yükleyin

```bash
dotnet add package prometheus-net.AspNetCore
```

### 2. Program.cs'i Güncelleyin

```csharp
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// ... diğer servisler

var app = builder.Build();

// Prometheus metrics middleware'ini ekleyin
app.UseHttpMetrics(); // HTTP request/response metrics

// ... diğer middleware'ler

// Metrics endpoint'ini expose edin
app.MapMetrics(); // /metrics endpoint'i

app.Run();
```

### 3. Özel Metrikler Ekleme (Opsiyonel)

```csharp
using Prometheus;

// Counter örneği
private static readonly Counter ProcessedJobCount = Metrics
    .CreateCounter("myapp_jobs_processed_total", "Toplam işlenen job sayısı");

// Gauge örneği
private static readonly Gauge JobsInQueue = Metrics
    .CreateGauge("myapp_jobs_queued", "Kuyruktaki job sayısı");

// Histogram örneği
private static readonly Histogram OrderValue = Metrics
    .CreateHistogram("myapp_order_value_usd", "Sipariş değerleri USD");

// Kullanım
ProcessedJobCount.Inc();
JobsInQueue.Set(15);
OrderValue.Observe(125.50);
```

## RabbitMQ Metrics

RabbitMQ'nun Prometheus metrics plugin'i varsayılan olarak etkindir. Erişim:
- Port: 15692
- Endpoint: http://rabbitmq:15692/metrics

RabbitMQ Management UI: http://localhost:15672

## Yararlı Prometheus Sorguları

### HTTP Request Rate
```promql
rate(http_requests_received_total[5m])
```

### HTTP Request Duration (95th percentile)
```promql
histogram_quantile(0.95, rate(http_request_duration_seconds_bucket[5m]))
```

### Error Rate
```promql
rate(http_requests_received_total{code=~"5.."}[5m])
```

### RabbitMQ Queue Depth
```promql
rabbitmq_queue_messages
```

### Active Connections
```promql
rabbitmq_connections
```

## Grafana ile Entegrasyon

Daha gelişmiş görselleştirme için Grafana ekleyebilirsiniz:

```yaml
grafana:
  image: grafana/grafana:latest
  container_name: grafana.container
  ports:
    - "3000:3000"
  environment:
    - GF_SECURITY_ADMIN_USER=admin
    - GF_SECURITY_ADMIN_PASSWORD=${GRAFANA_PASSWORD}
  volumes:
    - grafana_data:/var/lib/grafana
      depends_on:
        - prometheus
      networks:
        - university-network
  ```

  Grafana'da Prometheus'u data source olarak ekleyin:
  - URL: http://prometheus:9090

## Troubleshooting

### Metrics endpoint'ler çalışmıyor
1. API'lerin `/metrics` endpoint'lerini kontrol edin
2. `prometheus-net.AspNetCore` paketinin yüklü olduğundan emin olun
3. `app.MapMetrics()` çağrısının yapıldığından emin olun

### Prometheus target'ları "down" görünüyor
1. Docker network bağlantısını kontrol edin
2. API'lerin ayakta olduğundan emin olun
3. Port numaralarını doğrulayın
4. `docker-compose logs prometheus` ile logları inceleyin

### Volume permission hatası
Windows'ta Docker Desktop kullanıyorsanız, `prometheus.yml` dosyasının yolu doğru olmalıdır:
```yaml
volumes:
  - ./prometheus/prometheus.yml:/etc/prometheus/prometheus.yml:ro
```

## Kaynaklar

- [Prometheus Documentation](https://prometheus.io/docs/)
- [prometheus-net Documentation](https://github.com/prometheus-net/prometheus-net)
- [Prometheus Best Practices](https://prometheus.io/docs/practices/)
