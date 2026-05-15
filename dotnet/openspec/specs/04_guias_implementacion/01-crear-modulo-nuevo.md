# 01 - Crear un Módulo Nuevo

**Sección**: Guías de Implementación | **Documento**: 01 de 04 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Proveer una guía paso a paso para crear un nuevo módulo de negocio usando la plantilla ASP.NET Core Module y configurarlo dentro de una solución.

### Casos de uso
- arrancar un nuevo dominio de negocio
- generar un módulo reutilizable
- preparar un módulo para integrarlo en APIs o microservicios

---

## 2. Pre-requisitos

- `dotnet SDK 8.0`
- templates instalados: `CompanyName.Templates`
- acceso al repositorio del framework
- editor de código (VS Code, Rider, Visual Studio)

---

## 3. Generar el módulo

### Comando
```bash
dotnet new CompanyNameLower-module -n OrderModule
```

### Resultado
Se genera `OrderModule.sln` con la siguiente estructura:

```
OrderModule.sln
src/
  OrderModule.Application/
  OrderModule.Dapper/
  OrderModule.Domain/
  OrderModule.Domain.Shared/
  OrderModule.EntityFrameworkCore/
  OrderModule.HttpApi/
  OrderModule.HttpApi.Host/
test/
  OrderModule.Application.Tests/
  OrderModule.Dapper.Tests/
  OrderModule.Domain.Tests/
  OrderModule.EntityFrameworkCore.Tests/
  OrderModule.HttpApi.Tests/
```

---

## 4. Primeros pasos dentro del módulo

### 4.1 Abrir solución
- Abre `OrderModule.sln`
- Verifica que los proyectos compilen

### 4.2 Configurar `Program.cs` del host
En `OrderModule.HttpApi.Host`, revisa que el pipeline de ASP.NET Core contenga:
- logging
- swagger
- autenticación/authorization (si aplica)
- `AddCaching`, `AddEventBusLocal`, `AddValidation`

### 4.3 Crear la entidad de dominio
En `OrderModule.Domain` crea:
```csharp
public class Order : AggregateRoot<int>
{
    public string Number { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Order() { }

    public static Order Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new DomainException("Order number is required.");

        return new Order
        {
            Number = number,
            CreatedAt = DateTime.UtcNow
        };
    }
}
```

---

## 5. Agregar repositorio y especificación

### 5.1 Definir interfaz
En `OrderModule.Domain`:
```csharp
public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByNumberAsync(string number);
}
```

### 5.2 Implementar repositorio EF Core
En `OrderModule.EntityFrameworkCore`:
```csharp
public class OrderRepository : EfCoreRepository<Order>, IOrderRepository
{
    public OrderRepository(MyDbContext context) : base(context) { }

    public async Task<Order?> GetByNumberAsync(string number)
    {
        var spec = new OrderByNumberSpecification(number);
        return await FirstOrDefaultAsync(spec);
    }
}
```

### 5.3 Crear spec
En `OrderModule.Domain`:
```csharp
public class OrderByNumberSpecification : Specification<Order>
{
    public OrderByNumberSpecification(string number)
    {
        Query.Where(o => o.Number == number);
    }
}
```

---

## 6. Crear Application Service

En `OrderModule.Application`:
```csharp
public class CreateOrderService : ApplicationService
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    [UnitOfWork]
    public async Task<Result<OrderDto>> Handle(CreateOrderCommand command)
    {
        var order = Order.Create(command.Number);
        await _orderRepository.AddAsync(order);

        return Ok(new OrderDto { Id = order.Id, Number = order.Number });
    }
}
```

---

## 7. Crear endpoint HTTP

En `OrderModule.HttpApi` o `OrderModule.HttpApi.Host`:
```csharp
[ApiController]
[Route("api/orders")]
public class OrdersController : CompanyNameController
{
    private readonly CreateOrderService _createOrderService;

    public OrdersController(CreateOrderService createOrderService)
    {
        _createOrderService = createOrderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderCommand command)
    {
        var result = await _createOrderService.Handle(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }
}
```

---

## 8. Registrar dependencias

En el `Startup` o `Program.cs` del host:
```csharp
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<CreateOrderService>();
```

---

## 9. Primer CRUD y prueba

### Comando
```bash
dotnet test OrderModule.sln
```

### Verificación
- El proyecto compila
- El servicio de aplicación crea órdenes
- El endpoint responde 201 OK cuando la petición es válida

---

## 10. Checklist de completitud

- [ ] Módulo generado y solución abierta
- [ ] Entidad `Order` creada
- [ ] Repositorio y especificación implementados
- [ ] Application Service creado
- [ ] Endpoint HTTP expuesto
- [ ] Dependencias registradas en DI
- [ ] Pruebas básicas ejecutadas exitosamente
