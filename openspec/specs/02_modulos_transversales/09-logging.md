# 09 - Logging & Application Insights

**Sección**: Módulos Transversales | **Documento**: 09 de 10 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Proveer observabilidad estructurada y telemetría con:
- logging centralizado
- Application Insights para telemetría de requests y excepciones
- correlación de trazas

### Casos de uso
- depuración de errores en producción
- análisis de rendimiento
- seguimiento de solicitudes
- identificación de anomalías

### Stack tecnológico
- `Microsoft.Extensions.Logging`
- `Microsoft.ApplicationInsights.AspNetCore`
- `ApplicationInsights` SDK

---

## 2. Abstracciones Principales

### Interfaces clave
- `ILogger<T>`
- `ITelemetryClient`
- `IRequestTelemetryInitializer`

### Clases base
- `ApplicationInsightsTelemetryInitializer`
- `StructuredLogFormatter`

### Namespaces
- `CompanyName.ApplicationInsights.AspNetCore`
- `CompanyName.Logging`

---

## 3. Comportamiento Default

### Implementación out-of-box
- `ILogger` con formato estructurado
- integración con Application Insights
- logging de requests, excepciones y eventos de seguimiento

### Reglas predeterminadas
- los logs capturan `CorrelationId`
- se registran excepciones no manejadas
- los eventos de Application Insights son enriquecidos con metadata

---

## 4. Configuración en Program.cs

```csharp
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddApplicationInsights();
```

---

## 5. Patrones de Uso

### Inyección de dependencias
```csharp
public class OrderService
{
    private readonly ILogger<OrderService> _logger;

    public OrderService(ILogger<OrderService> logger)
    {
        _logger = logger;
    }

    public void ProcessOrder(int id)
    {
        _logger.LogInformation("Processing order {OrderId}", id);
    }
}
```

### Uso práctico
```csharp
_logger.LogError(ex, "Failed to process order {OrderId}", orderId);
```

### Best practices
- usa `LogInformation`, `LogWarning`, `LogError` según gravedad
- no loguees datos sensibles
- usa `BeginScope` para correlación de contexto

---

## 6. Extensión Personalizada

### Interfaces a implementar
- `ITelemetryClient`
- `IRequestTelemetryInitializer`

### Registro personalizado
```csharp
services.AddSingleton<ITelemetryClient, CustomTelemetryClient>();
```

### Ejemplo step-by-step
1. Implementa custom telemetry initializer
2. Registra en DI
3. Añade propiedades globales en cada telemetry item

---

## 7. Testing

### Mock/stub recommendations
- Mock `ILogger<T>`
- Mock `ITelemetryClient`

### Ejemplo de test
```csharp
[Fact]
public void ProcessOrder_LogsInformation()
{
    var loggerMock = new Mock<ILogger<OrderService>>();
    var service = new OrderService(loggerMock.Object);

    service.ProcessOrder(1);

    loggerMock.VerifyLog(LogLevel.Information, "Processing order 1", Times.Once());
}
```