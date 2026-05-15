# 03 - ServiceBus (Local + Azure)

**Sección**: Módulos Transversales | **Documento**: 03 de 10 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Ofrecer una solución para colas y tópicos de mensajería, con soporte para:
- entornos locales de desarrollo
- Azure Service Bus en producción
- patterns de enrutamiento y procesamiento asincrónico

### Casos de uso
- procesamiento en background
- integración entre servicios
- envío de notificaciones y tareas programadas
- desacople entre productores y consumidores

### Stack tecnológico
- `CompanyName.ServiceBus`
- `CompanyName.ServiceBus.Azure`
- `Azure.Messaging.ServiceBus`

---

## 2. Abstracciones Principales

### Interfaces clave
- `IServiceBusClient`
- `IServiceBusSender`
- `IServiceBusReceiver`
- `IMessageHandler<TMessage>`

### Clases base
- `ServiceBusClientBase`
- `MessageHandlerBase<TMessage>`

### Namespaces
- `CompanyName.ServiceBus`
- `CompanyName.ServiceBus.Azure`

---

## 3. Comportamiento Default

### Implementación out-of-box
- `LocalServiceBusClient`: encola mensajes localmente
- `AzureServiceBusClient`: envía/recibe mensajes de Azure
- `DefaultMessageDispatcher`: despacha mensajes a handlers

### Reglas predeterminadas
- retries automáticos configurables
- dead-letter en Azure al exceder reintentos
- mensajes serializados en JSON

---

## 4. Configuración en Program.cs

```csharp
builder.Services.AddServiceBusLocal();

// o
builder.Services.AddServiceBusAzure(builder.Configuration);
```

```csharp
public static class ServiceBusServiceCollectionExtensions
{
    public static IServiceCollection AddServiceBusLocal(this IServiceCollection services)
    {
        services.AddSingleton<IServiceBusClient, LocalServiceBusClient>();
        services.AddSingleton<IMessageDispatcher, LocalMessageDispatcher>();
        return services;
    }

    public static IServiceCollection AddServiceBusAzure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureServiceBusOptions>(configuration.GetSection("AzureServiceBus"));
        services.AddSingleton<IServiceBusClient, AzureServiceBusClient>();
        services.AddSingleton<IMessageDispatcher, AzureMessageDispatcher>();
        return services;
    }
}
```

---

## 5. Patrones de Uso

### Inyección de dependencias
```csharp
public class OrderSubmittedHandler : IMessageHandler<OrderSubmittedMessage>
{
    public async Task HandleAsync(OrderSubmittedMessage message)
    {
        // procesamiento asincrónico
    }
}
```

### Uso práctico
```csharp
await _serviceBusClient.SendAsync(new OrderSubmittedMessage { OrderId = order.Id });
```

### Best practices
- Diseña mensajes inmutables
- Implementa idempotencia en handlers
- Usa dead-letter para mensajes no procesables
- Define TTL apropiado para cada cola

---

## 6. Extensión Personalizada

### Interfaces a implementar
- `IServiceBusClient`
- `IMessageHandler<TMessage>`
- `IMessageSerializer`

### Registro personalizado
```csharp
services.AddSingleton<IServiceBusClient, CustomServiceBusClient>();
services.AddSingleton<IMessageDispatcher, CustomMessageDispatcher>();
```

### Ejemplo step-by-step
1. Implementa tu cliente de mensajería
2. Implementa dispatching de mensajes
3. Registra en DI
4. Mantén manejo de errores robusto

---

## 7. Testing

### Mock/stub recommendations
- Mock `IServiceBusClient`
- Mock `IMessageHandler<TMessage>`

### Ejemplo de test
```csharp
[Fact]
public async Task SendMessage_UsesServiceBusClient()
{
    var busMock = new Mock<IServiceBusClient>();
    var sender = new OrderMessageSender(busMock.Object);

    await sender.SendOrderSubmittedAsync(orderId);

    busMock.Verify(x => x.SendAsync(It.IsAny<OrderSubmittedMessage>()), Times.Once);
}
```
