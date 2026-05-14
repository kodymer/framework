# 02 - EventBus (Local + Azure)

**Sección**: Módulos Transversales | **Documento**: 02 de 10 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Permitir la publicación y suscripción de eventos de dominio e integración con soporte:
- local en-proceso para pruebas y desarrollo
- Azure Service Bus para producción distribuida

### Casos de uso
- sincronizar microservicios
- publicar cambios de estado de dominio
- ejecutar workflows asincrónicos
- desacoplar subsistemas

### Stack tecnológico
- `CompanyName.EventBus`
- `CompanyName.EventBus.Azure`
- `Azure.Messaging.ServiceBus`

---

## 2. Abstracciones Principales

### Interfaces clave
- `IEventBus`
- `IEventHandler<TEvent>`
- `IDomainEvent`
- `IIntegrationEvent`

### Clases base
- `EventBusBase`
- `DomainEventHandlerBase<TEvent>`

### Namespaces
- `CompanyName.EventBus`
- `CompanyName.EventBus.Azure`

---

## 3. Comportamiento Default

### Implementación out-of-box
- `LocalEventBus`: publica eventos inmediatamente en memoria
- `AzureServiceBusEventBus`: envía mensajes a Azure Service Bus
- `DefaultEventHandlerRegistry`: encuentra handlers por reflexión

### Reglas predeterminadas
- eventos de dominio se envían tras commit de UoW
- mensajes serializados en JSON
- retries configurables en Azure

---

## 4. Configuración en Program.cs

```csharp
builder.Services.AddEventBusLocal();
```

```csharp
public static class EventBusServiceCollectionExtensions
{
    public static IServiceCollection AddEventBusLocal(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus, LocalEventBus>();
        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes.AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
        return services;
    }

    public static IServiceCollection AddEventBusAzure(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IEventBus, AzureServiceBusEventBus>();
        services.Configure<AzureServiceBusOptions>(config.GetSection("AzureServiceBus"));
        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes.AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
        return services;
    }
}
```

---

## 5. Patrones de Uso

### Inyección de dependencias
```csharp
public class OrderCreatedHandler : IEventHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent evt)
    {
        // reaccionar al evento
        return Task.CompletedTask;
    }
}
```

### Uso práctico
```csharp
await _eventBus.PublishAsync(new OrderCreatedEvent(order.Id));
```

### Best practices
- Evita lógica pesada en handlers
- Mantén events pequeños y explícitos
- Usa eventos de integración para boundaries de servicios
- Diseña idempotencia en handlers

---

## 6. Extensión Personalizada

### Interfaces a implementar
- `IEventBus`
- `IEventHandler<TEvent>`
- `IEventSerializer`

### Registro personalizado
```csharp
services.AddSingleton<IEventBus, CustomEventBus>();
services.AddSingleton<IEventSerializer, CustomEventJsonSerializer>();
```

### Ejemplo step-by-step
1. Crea `CustomEventBus : IEventBus`
2. Implementa `PublishAsync` y `SubscribeAsync`
3. Registra el bus en DI
4. Mantén payloads lean

---

## 7. Testing

### Mock/stub recommendations
- Mock `IEventBus`
- Verifica `PublishAsync` llamadas

### Ejemplo de test
```csharp
[Fact]
public async Task PublishOrderCreatedEvent_WhenOrderCreated()
{
    var eventBusMock = new Mock<IEventBus>();
    var service = new OrderService(eventBusMock.Object);

    await service.CreateOrderAsync(orderCmd);

    eventBusMock.Verify(bus => bus.PublishAsync(It.IsAny<OrderCreatedEvent>()), Times.Once);
}
```
