# 05 - Domain & Application Base Services

**Sección**: Módulos Transversales | **Documento**: 05 de 10 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Proporcionar clases base para:
- servicios de dominio
- servicios de aplicación
- reducir boilerplate repetitivo
- centralizar concerns transversales como logging, validación y auditoría

### Casos de uso
- implementación de use cases comunes
- services de dominio que encapsulan reglas del negocio
- service layer que coordina repositorios y eventos

### Stack tecnológico
- Herencia de clases base
- Autofac / Microsoft DI
- patrones de interceptores y AOP

---

## 2. Abstracciones Principales

### Interfaces clave
- `IDomainService`
- `IApplicationService`
- `IServiceResult`

### Clases base
- `DomainService`
- `ApplicationService`

### Namespaces
- `CompanyName.Ddd.Domain`
- `CompanyName.Ddd.Application`

---

## 3. Comportamiento Default

### Implementación out-of-box
- `DomainService` proporciona helpers comunes al dominio
- `ApplicationService` expone logging, current user, validación y UoW
- interceptores aplican `UnitOfWork` automáticamente

### Reglas predeterminadas
- usar la clase base como primer paso
- evitar duplicar validaciones en servicios concretos
- escribir servicios ligeros en lógica, no en infraestructura

---

## 4. Configuración en Program.cs

```csharp
services.AddScoped<IDomainService, DomainService>();
services.AddScoped<IApplicationService, ApplicationService>();
```

```csharp
public static class ServiceBaseCollectionExtensions
{
    public static IServiceCollection AddBaseServices(this IServiceCollection services)
    {
        services.AddScoped<ApplicationService, ApplicationService>();
        services.AddScoped<DomainService, DomainService>();
        return services;
    }
}
```

---

## 5. Patrones de Uso

### Inyección de dependencias
```csharp
public class CreateOrderService : ApplicationService
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
}
```

### Uso práctico
```csharp
public class CreateOrderService : ApplicationService
{
    [UnitOfWork]
    public async Task<Result<OrderDto>> Handle(CreateOrderCommand command)
    {
        var order = Order.Create(...);
        await _orderRepository.AddAsync(order);
        return Ok(MapToDto(order));
    }
}
```

### Best practices
- No renombres la clase base
- Extiende solo cuando necesitas shared behavior
- Mantén los métodos concretos del servicio sencillos

---

## 6. Extensión Personalizada

### Interfaces a implementar
- `IApplicationService` para servicios de aplicación custom
- `IDomainService` para servicios de dominio custom

### Registro personalizado
```csharp
services.AddTransient<IOrderCreationService, CustomCreateOrderService>();
```

### Ejemplo step-by-step
1. Extiende `ApplicationService`
2. Inyecta repositorios y dependencias
3. Usa `[UnitOfWork]` para manejar transacción automática
4. Devuelve `Result<T>` en lugar de exceptions en la capa de aplicación

---

## 7. Testing

### Mock/stub recommendations
- Mock repositorios y event bus
- Testea solo la lógica de orquestación

### Ejemplo de test
```csharp
[Fact]
public async Task Handle_CreatesOrder_WhenCommandValid()
{
    var repoMock = new Mock<IOrderRepository>();
    var busMock = new Mock<IEventBus>();
    var service = new CreateOrderService(repoMock.Object, busMock.Object);

    var result = await service.Handle(new CreateOrderCommand(...));

    Assert.True(result.IsSuccess);
    repoMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
}
```
