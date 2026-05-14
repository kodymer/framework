# 2. Principios Arquitectónicos - CompanyName Framework

**Sección**: Visión y Arquitectura | **Documento**: 02 de 04 | **Estado**: v1.0.0-rc

---

## 🎯 Principios Fundamentales

El CompanyName Framework se construye sobre principios sólidos de ingeniería de software. Estos principios guían **todas** las decisiones arquitectónicas y de diseño.

---

## 1️⃣ Domain-Driven Design (DDD)

### Principio
**"El código debe reflejar el dominio de negocio de manera clara y directa."**

### Aplicación
- **Ubiquitous Language**: Nombres en dominio, no técnicos
  ```csharp
  // ✅ Correcto - lenguaje de negocio
  var order = Order.Create(orderNumber, items);
  
  // ❌ Incorrecto - jerga técnica
  var obj = CreateDatabaseRecord(dto);
  ```

- **Aggregate Roots**: Entidades que mantienen invariantes
  ```csharp
  public class Order : AggregateRoot
  {
      // Order es responsable de mantener consistencia
      public void AddLine(OrderLine line)
      {
          if (this.Lines.Sum(l => l.Quantity) + line.Quantity > MAX_ITEMS)
              throw new DomainException("Too many items");
          
          this.Lines.Add(line);
      }
  }
  ```

- **Value Objects**: Objetos sin identidad, enfocados en valores
  ```csharp
  public class OrderNumber : ValueObject
  {
      public string Value { get; }
      
      public static OrderNumber From(string value)
      {
          if (string.IsNullOrEmpty(value))
              throw new DomainException("OrderNumber cannot be empty");
          
          return new OrderNumber(value);
      }
  }
  ```

- **Domain Services**: Lógica que cruza múltiples agregates
  ```csharp
  public class OrderShippingService : IDomainService
  {
      public async Task ShipOrderAsync(Order order, IShippingProvider provider)
      {
          var shipment = await provider.CreateShipmentAsync(order);
          order.MarkAsShipped(shipment.TrackingNumber);
      }
  }
  ```

### Beneficio
Código que developers de negocio pueden leer. Menos "impedance mismatch" entre dominio y código.

---

## 2️⃣ Separation of Concerns (SoC)

### Principio
**"Cada capa tiene una responsabilidad única y clara."**

### Aplicación

| Capa | Responsabilidad | Ejemplo |
|------|-----------------|---------|
| **Presentation** | HTTP request/response | Controllers, Endpoints |
| **Application** | Orquestar use cases | Application Services, DTOs |
| **Domain** | Reglas de negocio | Entities, Value Objects, Services |
| **Data Access** | Persistencia | Repositories, DbContext |
| **Infrastructure** | Cross-cutting concerns | Cache, Events, Logging, Auditing |

### Violaciones Comunes a Evitar
```csharp
// ❌ VIOLACIÓN: Lógica de negocio en Controller
public class OrdersController : Controller
{
    [HttpPost]
    public IActionResult Create(OrderDto dto)
    {
        if (dto.Items.Count == 0) return BadRequest();
        var order = new Order { ... };
        _db.Orders.Add(order);
        _db.SaveChanges();
        return Ok(order.Id);
    }
}

// ✅ CORRECTO: Separación clara
public class CreateOrderService : ApplicationService
{
    public async Task<Result<OrderDto>> Handle(CreateOrderCommand cmd)
    {
        var order = Order.Create(cmd.OrderNumber, cmd.Items);
        await _repository.AddAsync(order);
        return Ok(MapToDto(order));
    }
}
```

### Beneficio
Código más testeable, mantenible, y con menor acoplamiento.

---

## 3️⃣ Dependency Inversion (SOLID - D)

### Principio
**"Depende de abstracciones, no de implementaciones concretas."**

### Aplicación

```csharp
// ❌ VIOLACIÓN: Acoplamiento a implementación concreta
public class OrderService
{
    private SqlServerOrderRepository _repo;
    private RedisCache _cache;
    
    public OrderService()
    {
        _repo = new SqlServerOrderRepository();
        _cache = new RedisCache();
    }
}

// ✅ CORRECTO: Acoplamiento a abstracciones
public class OrderService
{
    private IOrderRepository _repo;
    private IDistributedCache _cache;
    
    public OrderService(IOrderRepository repo, IDistributedCache cache)
    {
        _repo = repo;
        _cache = cache;
    }
}
```

### En el Framework
- **Marker Interfaces** para auto-wiring:
  ```csharp
  public class OrderService : IApplicationService  // ← Marker
  {
      // Auto-registrada en Autofac como ApplicationService
  }
  ```

- **Abstracciones explícitas**:
  ```csharp
  public interface IRepository<T> where T : AggregateRoot
  {
      Task<T> FirstOrDefaultAsync(ISpecification<T> spec);
      Task AddAsync(T entity);
  }
  ```

### Beneficio
Fácil reemplazar implementaciones (Local ↔ Azure, SQL Server ↔ PostgreSQL).

---

## 4️⃣ Single Responsibility Principle (SOLID - S)

### Principio
**"Una clase debe tener una razón única para cambiar."**

### Aplicación

| Clase | Responsabilidad | Cambio Causado Por |
|-------|-----------------|-------------------|
| `Order` | Mantener invariantes de orden | Cambios en reglas de negocio |
| `CreateOrderService` | Orquestar creación de orden | Cambios en el use case |
| `OrderRepository` | Persistencia de órdenes | Cambios en BD/ORM |
| `OrderValidator` | Validar datos de entrada | Cambios en reglas de validación |

### Violación Común
```csharp
// ❌ RESPONSABILIDAD MÚLTIPLE
public class OrderManager
{
    // 1. Crear orden (lógica de negocio)
    public void Create(OrderDto dto) { ... }
    
    // 2. Guardar en BD (persistencia)
    public void SaveToDb(Order order) { ... }
    
    // 3. Enviar email (notificaciones)
    public void SendConfirmationEmail(Order order) { ... }
    
    // 4. Log en Azure (logging)
    public void LogToAzure(Order order) { ... }
}

// ✅ SEPARACIÓN CORRECTA
public class CreateOrderService : ApplicationService
{
    public async Task Handle(CreateOrderCommand cmd)
    {
        var order = Order.Create(...);
        await _repository.AddAsync(order);
        await _eventBus.PublishAsync(new OrderCreatedEvent(order.Id));
        // Los handlers del evento se encargan del resto
    }
}

public class SendOrderConfirmationEmailHandler : IEventHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent evt)
    {
        await _emailService.SendAsync(...);
    }
}
```

### Beneficio
Cada clase es más pequeña, más fácil de testear, menos acoplada.

---

## 5️⃣ Convention Over Configuration

### Principio
**"Hay un "camino feliz" convenido. Sólo configura lo diferente."**

### Aplicación

#### Naming Conventions
```
Carpeta: Entities/         → Clases de dominio
Carpeta: Services/         → Application Services
Carpeta: Repositories/     → Repositorios
Carpeta: Validators/       → Validadores
Carpeta: Endpoints/        → HTTP endpoints

Namespace: YourModule.Domain
Namespace: YourModule.Application
Namespace: YourModule.EntityFrameworkCore
```

#### Patrón de Clase Base
```csharp
// Convention: Heredar de base class
public class CreateOrderService : ApplicationService
{
    // Heredas automáticamente:
    // - Logger
    // - CurrentUser
    // - Validation
    // - Auditing
    // - UnitOfWork
}

// No necesitas decoradores/atributos para cada una
[Authorize]
[ValidateInput]
[LogActivity]
[Audit]
[Transaction]
public async Task Handle(...) { }  // ← Sin framework base

// vs

[UnitOfWork]  // ← Uno solo, hereda todo
public async Task Handle(...) { }  // ← Con framework base
```

#### Rutas de API
```csharp
// Convention: /api/[Controller]/[Action]
public class OrdersEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/orders", Create);  // ← Automático basado en nombre
}
```

### Beneficio
Menos boilerplate, más consistencia, onboarding más rápido.

---

## 6️⃣ Extensibilidad Sin Modificación (Open/Closed Principle)

### Principio
**"Abierto para extensión, cerrado para modificación."**

### Aplicación

#### No Modificar Framework Base
```csharp
// ❌ NO HAGAS ESTO: Modificar clase base
// companyname.core/src/ApplicationService.cs
public class ApplicationService
{
    // ← Agregar campos/métodos rompería todos los servicios
}

// ✅ HAZ ESTO: Extender sin modificar
public class OrderService : ApplicationService
{
    // Personalizar comportamiento vía herencia
    
    protected override void LogBeforeExecute(object request)
    {
        base.LogBeforeExecute(request);
        // Tu logging adicional
    }
}

// O vía decoradores
public class OrderServiceWithCustomLogging : IOrderService
{
    private readonly IOrderService _inner;
    
    public async Task Handle(CreateOrderCommand cmd)
    {
        // Tu logging aquí
        return await _inner.Handle(cmd);
    }
}
```

#### Reemplazar Módulos Sin Tocar Código
```csharp
// Development: EventBus local
builder.Services.AddEventBusLocal();

// Production: Azure Service Bus
builder.Services.AddEventBusAzure(builder.Configuration);

// El código de aplicación es idéntico:
await _eventBus.PublishAsync(evt);  // ← Mismo código
```

### Beneficio
Evolucionar framework sin breaking changes. Múltiples implementaciones coexistentes.

---

## 7️⃣ Fail Fast, Fail Safe

### Principio
**"Detecta problemas temprano. Recuperate gracefully."**

### Aplicación

#### Fail Fast: Validación en Boundaries
```csharp
// En ValueObject: Validar temprano
public class OrderNumber : ValueObject
{
    public static OrderNumber From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("OrderNumber cannot be empty");  // ← Temprano
        
        if (value.Length > 50)
            throw new DomainException("OrderNumber too long");
        
        return new OrderNumber(value);
    }
}

// En Aggregate: No aceptar estados inválidos
public class Order : AggregateRoot
{
    public void AddLine(OrderLine line)
    {
        if (line.Quantity <= 0)
            throw new DomainException("Quantity must be positive");  // ← Fail fast
        
        this.Lines.Add(line);
    }
}
```

#### Fail Safe: Manejo de Errores
```csharp
// Result pattern: No lanzar en happy path
public class CreateOrderService : ApplicationService
{
    [UnitOfWork]
    public async Task<Result<OrderDto>> Handle(CreateOrderCommand cmd)
    {
        // Validaciones
        if (!cmd.IsValid())
            return Fail("Invalid command");
        
        // Lógica
        try
        {
            var order = Order.Create(...);
            await _repository.AddAsync(order);
            return Ok(MapToDto(order));
        }
        catch (DomainException ex)
        {
            return Fail(ex.Message);  // ← Fail safe
        }
    }
}

// En endpoint
private async Task<IResult> Handle(CreateOrderCommand cmd, CreateOrderService svc)
{
    var result = await svc.Handle(cmd);
    return result.IsSuccess 
        ? Results.Ok(result.Value)
        : Results.BadRequest(result.Errors);  // ← Graceful handling
}
```

### Beneficio
Sistemas resilientes. Errores capturados en el lugar correcto, no esparcidos.

---

## 8️⃣ Testability First

### Principio
**"El código debe ser fácil de testear desde el diseño."**

### Aplicación

#### Inyectar Dependencias
```csharp
// ❌ DIFÍCIL DE TESTEAR: Crear dependencias internamente
public class CreateOrderService
{
    public async Task Handle(CreateOrderCommand cmd)
    {
        var repo = new SqlServerOrderRepository();  // ← Imposible mockar
        var cache = new RedisCache();               // ← Imposible mockar
        
        var order = Order.Create(cmd.OrderNumber);
        await repo.AddAsync(order);
    }
}

// ✅ FÁCIL DE TESTEAR: Inyectar dependencias
public class CreateOrderService
{
    private readonly IOrderRepository _repo;
    private readonly IDistributedCache _cache;
    
    public CreateOrderService(IOrderRepository repo, IDistributedCache cache)
    {
        _repo = repo;
        _cache = cache;
    }
    
    public async Task Handle(CreateOrderCommand cmd)
    {
        // ← Mockar es trivial en tests
    }
}

// En test
[Fact]
public async Task ShouldCreateOrder()
{
    var mockRepo = new Mock<IOrderRepository>();
    var mockCache = new Mock<IDistributedCache>();
    
    var svc = new CreateOrderService(mockRepo.Object, mockCache.Object);
    var result = await svc.Handle(new CreateOrderCommand("ORD-001"));
    
    Assert.True(result.IsSuccess);
    mockRepo.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
}
```

### Beneficio
Tests genuinos. Cobertura real. Confianza en el código.

---

## 9️⃣ Progressive Enhancement

### Principio
**"Comienza simple. Añade sofisticación cuando la necesites."**

### Aplicación

#### CRUD Básico Primero
```csharp
// Fase 1: CRUD simple sin complejidad
public class Order
{
    public int Id { get; set; }
    public string Number { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

#### Luego Capas DDD
```csharp
// Fase 2: Agregate root y value objects
public class Order : AggregateRoot
{
    public OrderNumber Number { get; private set; }
    public OrderStatus Status { get; private set; }
    public List<OrderLine> Lines { get; private set; }
}

public class OrderNumber : ValueObject { ... }
```

#### Finalmente Events y Sagas
```csharp
// Fase 3: Event sourcing y sagas
public class Order : AggregateRoot
{
    public void CreatedAt() => RaiseDomainEvent(new OrderCreatedEvent(...));
}

public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent evt) => ...
}
```

### Beneficio
Bajo costo inicial. Escalas arquitectura conforme crece complejidad.

---

## 🎓 Matriz de Principios vs Prácticas

| Principio | Práctica en el Framework |
|-----------|--------------------------|
| DDD | Aggregates, Value Objects, Domain Services |
| SoC | 6 capas claras con responsabilidades |
| Dependency Inversion | DI via Autofac, Marker Interfaces |
| Single Responsibility | Cada clase una razón para cambiar |
| Convention over Config | Naming, folder structure, base classes |
| Open/Closed | Reemplazar módulos sin modificar |
| Fail Fast/Safe | Validación temprana, Result pattern |
| Testability | Inyección de deps, interfaces |
| Progressive Enhancement | Comenzar simple, escalar gradualmente |

---

## ✅ Checklist: ¿Estoy siguiendo estos principios?

- [ ] Mi clase tiene UNA responsabilidad única
- [ ] Dependencias son inyectadas, no creadas internamente
- [ ] Uso Value Objects para conceptos de negocio
- [ ] Aggregate Roots mantienen sus invariantes
- [ ] Valido temprano en boundaries
- [ ] Código es fácil de testear
- [ ] No violo capas (Application no importa Data Access)
- [ ] Uso nombres del dominio, no jerga técnica
- [ ] Código es extensible sin modificación
- [ ] Manejo de errores es graceful (Result pattern)

---

## 🔗 Próximos Documentos

1. **03 - Capas de Arquitectura** → Cómo se implementan estos principios
2. **04 - Patrones DDD** → Tácticas específicas para cada principio

---

**Versión**: v1.0.0-rc | **Fecha**: 2026-05-13 | **Anterior**: [01 - Visión General](01-vision-general.md) | **Próximo**: [03 - Capas de Arquitectura](03-capas-arquitectura.md)
