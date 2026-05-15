# 02 - Extender el Framework

**Sección**: Guías de Implementación | **Documento**: 02 de 04 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Guiar la creación de un nuevo módulo transversal del framework y su integración en el pipeline de extensiones.

### Casos de uso
- añadir nueva funcionalidad reusable
- crear un mecanismo de integración nuevo
- extender los comportamientos del framework sin modificar el core

---

## 2. Cuándo crear un módulo transversal

### Usa un módulo transversal cuando:
- la funcionalidad se usa en varios proyectos
- la lógica pertenece a infraestructura compartida
- necesitas un punto único de configuración

### No lo uses cuando:
- la lógica es específica de un único servicio
- es stricly UI o un feature aislado de un solo proyecto

---

## 3. Patrón de extensión

### 3.1 Abstracción
Define interfaces y contratos en un proyecto core o abstraído.

### 3.2 Implementación
Crea la implementación por defecto en un proyecto separado.

### 3.3 Registro
Expón un método `AddXxx` en `IServiceCollection`.

### 3.4 Consumo
Permite que cualquier host use la implementación a través de la interfaz.

---

## 4. Paso a paso

### 4.1 Identificar la abstracción
Ejemplo: un nuevo sistema de métricas.

Define la interfaz en `CompanyName.Core` o un módulo compartido:
```csharp
public interface IMetricPublisher
{
    Task PublishAsync(string name, double value);
}
```

### 4.2 Implementación por defecto
En `CompanyName.Metrics`:
```csharp
public class ApplicationInsightsMetricPublisher : IMetricPublisher
{
    private readonly TelemetryClient _client;

    public ApplicationInsightsMetricPublisher(TelemetryClient client)
    {
        _client = client;
    }

    public Task PublishAsync(string name, double value)
    {
        _client.GetMetric(name).TrackValue(value);
        return Task.CompletedTask;
    }
}
```

### 4.3 Registrar el módulo
```csharp
public static class MetricsServiceCollectionExtensions
{
    public static IServiceCollection AddMetrics(this IServiceCollection services)
    {
        services.AddSingleton<IMetricPublisher, ApplicationInsightsMetricPublisher>();
        return services;
    }
}
```

### 4.4 Usar el módulo
En código de aplicación:
```csharp
public class OrderService
{
    private readonly IMetricPublisher _metrics;

    public OrderService(IMetricPublisher metrics)
    {
        _metrics = metrics;
    }

    public async Task ProcessAsync()
    {
        await _metrics.PublishAsync("orders.processed", 1);
    }
}
```

---

## 5. Testing del módulo

### 5.1 Test de interfaz
```csharp
[Fact]
public async Task PublishAsync_SendsMetric()
{
    var telemetryMock = new Mock<TelemetryClient>();
    var publisher = new ApplicationInsightsMetricPublisher(telemetryMock.Object);

    await publisher.PublishAsync("test", 1);

    // Verifica que se llamó al cliente de telemetría
}
```

### 5.2 Test de integración
- verifica que la implementación se registre correctamente en DI
- consume `IMetricPublisher` desde un host minimal

---

## 6. Documentar el módulo

### Incluye en la documentación:
- objetivo del módulo
- interfaces públicas
- métodos `AddXxx`
- ejemplos de Program.cs
- cómo probar y extender

### Ejemplo de README interno
```
# Metrics Module

Este módulo expone métricas estructuradas que pueden publicarse en Application Insights o en otro backend.
```

---

## 7. Checklist de extensión

- [ ] Definí la abstracción en un namespace compartido
- [ ] Implementé la versión por defecto
- [ ] Registré el servicio con `AddXxx`
- [ ] Proporcioné documentación básica
- [ ] Añadí tests unitarios e integración
