# 3. Capas de Arquitectura - CompanyName Framework

**Sección**: Visión y Arquitectura | **Documento**: 03 de 04 | **Estado**: v1.0.0-rc

---

## 🏗️ Visión General de Capas

El CompanyName Framework implementa una arquitectura en **6 capas** claramente separadas, cada una con responsabilidades específicas:

```
┌─────────────────────────────────────────────────┐
│         PRESENTATION LAYER (ASP.NET Core)       │ ← │           HTTP, Controllers, Endpoints          │
├─────────────────────────────────────────────────┤
│         APPLICATION LAYER (DDD Services)        │ ← │          Use Cases, DTOs, Orchestration         │
├─────────────────────────────────────────────────┤
│          DOMAIN LAYER (Business Rules)          │ ← │         Entities, Value Objects, Logic          │
├─────────────────────────────────────────────────┤
│        DATA ACCESS LAYER (Persistence)          │ ← │        Repositories, DbContext, Queries         │
├─────────────────────────────────────────────────┤
│        INFRASTRUCTURE LAYER (Cross-Cutting)     │ ← │         Cache, Events, Auditing, Logging        │  
├─────────────────────────────────────────────────┤
│          CORE LAYER (Utilities & Base)          │ ← │         Extensions, DI Markers, Helpers         │
└─────────────────────────────────────────────────┘
```

### Regla de Oro: Las Dependencias van HACIA ABAJO ↓

```
Presentation → Application
Application → Domain
Domain → (sin dependencias externas)
Application → Data Access
Data Access → Infrastructure
```

---

## 1️⃣ Presentation Layer (ASP.NET Core)

### Responsabilidad
**Traducir HTTP request/response. NADA más.**

### Proyectos del Framework
- `CompanyName.AspNetCore` — Base configuration
- `CompanyName.AspNetCore.Abstractions` — Interfaces (IEndpoint)
- `CompanyName.AspNetCore.Mvc` — Controller base classes
- `CompanyName.ApplicationInsights.AspNetCore` — Telemetry

### Componentes Principales

#### A. Endpoints (Minimal APIs)
```csharp
public class CreateOrderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/orders", Handle)
           .WithName("CreateOrder")
           .WithOpenApi();
    
    private async Task<IResult> Handle(
        CreateOrderCommand cmd,
        CreateOrderService svc)
    {
        var result = await svc.Handle(cmd);
        return result.IsSuccess
            ? Results.Created($"/orders/{result.Value.Id}", result.Value)
            : Results.BadRequest(result.Errors);
    }
}
```

#### B. Controllers (MVC)
```csharp
public class OrdersController : CompanyNameController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
        var cmd = MapToCommand(dto);
        var result = await _service.Handle(cmd);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }
}
```

#### C. DTO Mapping
```csharp
[AutoMap(typeof(Order))]  // AutoMapper convention
public class OrderDto
{
    public int Id { get; set; }
    public string Number { get; set; }
    public List<OrderLineDto> Lines { get; set; }
}
```

### Responsabilidades Específicas
- ✅ Deserializar HTTP body → DTOs
- ✅ Validar formato HTTP (headers, content-type)
- ✅ Llamar a Application Services
- ✅ Serializar response → HTTP body
- ✅ Mapear status codes HTTP adecuados
- ✅ Loguear requests/responses

### Responsabilidades PROHIBIDAS
- ❌ Lógica de negocio
- ❌ Queries a BD
- ❌ Decisiones sobre persistencia
- ❌ Validaciones de dominio
- ❌ Transformaciones de datos

### Dependencias Típicas
```csharp
public class OrdersEndpoint : IEndpoint
{
    // Inyectar SÓLO: Application Services, Mappers
    private readonly CreateOrderService _createService;
    private readonly IMapper _mapper;
    
    // NUNCA inyectar: IRepository, DbContext, IEventBus
}
```

---

## 2️⃣ Application Layer (DDD Services)

### Responsabilidad
**Orquestar Use Cases. Traducir entre Presentation y Domain.**

### Proyectos del Framework
- `CompanyName.Ddd.Application` — Base classes, orchestration
- `CompanyName.AutoMapper` — DTO mapping configuration

### Componentes Principales

#### A. Application Services (Use Cases)
```csharp
public class CreateOrderService : ApplicationService  // ← Base class
{
    private readonly IOrderRepository _repo;
    private readonly IEventBus _eventBus;
    private readonly ICurrentUser _currentUser;
    
    public CreateOrderService(
        IOrderRepository repo,
        IEventBus eventBus,
        ICurrentUser currentUser)
    {
        _repo = repo;
        _eventBus = eventBus;
        _currentUser = currentUser;
    }
    
    [UnitOfWork]  // ← Transacción automática
    public async Task<Result<OrderDto>> Handle(CreateOrderCommand cmd)
    {
        // 1. Traducir DTO → Domain object
        var order = Order.Create(
            OrderNumber.From(cmd.OrderNumber),
            cmd.Items.Select(MapToDomainLine).ToList()
        );
        
        // 2. Persistir via Repository
        await _repo.AddAsync(order);
        
        // 3. Publicar eventos
        await _eventBus.PublishAsync(
            order.GetDomainEvents().ToList()
        );
        
        // 4. Traducir Domain → DTO
        var dto = new OrderDto
        {
            Id = order.Id,
            Number = order.Number.Value,
            CreatedBy = _currentUser.Id
        };
        
        return Ok(dto);
    }
}
```

#### B. Commands & Queries
```csharp
public record CreateOrderCommand(
    string OrderNumber,
    List<OrderLineCmd> Items
) : ICommand;

public record GetOrderQuery(int OrderId) : IQuery;

public class GetOrderHandler : IQueryHandler<GetOrderQuery, OrderDto>
{
    public async Task<OrderDto> Handle(GetOrderQuery query)
    {
        var spec = new GetOrderByIdSpecification(query.OrderId);
        var order = await _repo.FirstOrDefaultAsync(spec);
        return MapToDto(order);
    }
}
```

#### C. Validators
```csharp
public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.OrderNumber)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(x => x.Items)
            .NotEmpty()
            .Must(items => items.Count <= 100)
            .WithMessage("Maximum 100 items allowed");
    }
}
```

### Heredar Automáticamente (Base Class)
```csharp
// ApplicationService base proporciona:
public abstract class ApplicationService
{
    protected ILogger<TService> Logger { get; }  // ← Logging automático
    protected ICurrentUser CurrentUser { get; }  // ← Current user context
    protected IStringLocalizer Localizer { get; } // ← Localization
    protected IAuditPropertySetter Auditor { get; } // ← Auditing
    
    protected virtual void ValidateInput(object input) { }
    protected virtual void OnSuccess(object result) { }
    protected virtual void OnError(Exception ex) { }
}
```

### Responsabilidades Específicas
- ✅ Orquestar flujo del use case
- ✅ Traducir DTOs ↔ Domain objects
- ✅ Coordinar con múltiples repositorios
- ✅ Publicar eventos de dominio
- ✅ Validación de entrada
- ✅ Logging y auditoría

### Responsabilidades PROHIBIDAS
- ❌ Decisiones de negocio (van al Domain)
- ❌ Queries directo a BD (van a Repositories)
- ❌ HTTP concerns (van a Presentation)
- ❌ Logging a BD (va a Infrastructure)

### Dependencias Típicas
```csharp
public class MyApplicationService : ApplicationService
{
    // ✅ Inyectar: Repositories, Specifications, EventBus, Services
    private readonly IOrderRepository _orderRepo;
    private readonly IEventBus _eventBus;
    private readonly CreateOrderValidator _validator;
}
```

---

## 3️⃣ Domain Layer (Business Rules)

### Responsabilidad
**Encapsular las reglas de negocio. CERO dependencias de infraestructura.**

### Proyectos del Framework
- `CompanyName.Ddd.Domain` — Base classes, patterns
- `CompanyName.Ddd.Domain.EventBus` — Event definition
- `CompanyName.Security` — Current user context
- `CompanyName.Localization` — Localization

### Componentes Principales

#### A. Aggregate Roots
```csharp
public class Order : AggregateRoot<int>
{
    public OrderNumber Number { get; private set; }
    public OrderStatus Status { get; private set; }
    public List<OrderLine> Lines { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    // REGLA DE NEGOCIO 1: Order debe tener al menos una línea
    public static Order Create(OrderNumber number, List<OrderLine> lines)
    {
        if (lines == null || lines.Count == 0)
            throw new DomainException("Order must have at least one line");
        
        var order = new Order
        {
            Number = number,
            Status = OrderStatus.New,
            Lines = lines,
            CreatedAt = DateTime.UtcNow
        };
        
        // Evento de dominio: Order fue creada
        order.AddDomainEvent(new OrderCreatedEvent(order.Id, order.Number.Value));
        
        return order;
    }
    
    // REGLA DE NEGOCIO 2: Solo se pueden añadir líneas a órdenes nuevas
    public void AddLine(OrderLine line)
    {
        if (this.Status != OrderStatus.New)
            throw new DomainException("Cannot add lines to non-new orders");
        
        if (line.Quantity <= 0)
            throw new DomainException("Line quantity must be positive");
        
        this.Lines.Add(line);
        this.AddDomainEvent(new OrderLineAddedEvent(this.Id, line.ProductId));
    }
    
    // REGLA DE NEGOCIO 3: Order se puede confirmar solo si tiene líneas válidas
    public void Confirm()
    {
        if (this.Status != OrderStatus.New)
            throw new DomainException("Order already confirmed");
        
        if (this.Lines.Any(l => l.Quantity > 1000))
            throw new DomainException("Line quantities exceed maximum");
        
        this.Status = OrderStatus.Confirmed;
        this.AddDomainEvent(new OrderConfirmedEvent(this.Id));
    }
}
```

#### B. Value Objects
```csharp
public class OrderNumber : ValueObject
{
    private static readonly Regex ValidFormat = new Regex(@"^ORD-\d{6}$");
    
    public string Value { get; }
    
    // Factory method: Garantiza OrderNumber válido
    public static OrderNumber From(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new DomainException("OrderNumber cannot be empty");
        
        if (!ValidFormat.IsMatch(value))
            throw new DomainException($"OrderNumber format invalid: {value}");
        
        return new OrderNumber(value);
    }
    
    private OrderNumber(string value) => Value = value;
    
    // ValueObject: Se comparan por valor, no por identidad
    public override bool Equals(object obj) =>
        obj is OrderNumber on && on.Value == this.Value;
    
    public override int GetHashCode() => Value.GetHashCode();
}

public class OrderLine : ValueObject
{
    public int ProductId { get; }
    public int Quantity { get; }
    public decimal UnitPrice { get; }
    
    public static OrderLine Create(int productId, int quantity, decimal unitPrice)
    {
        if (productId <= 0) throw new DomainException("Invalid product");
        if (quantity <= 0) throw new DomainException("Quantity must be positive");
        if (unitPrice < 0) throw new DomainException("Price cannot be negative");
        
        return new OrderLine { ProductId = productId, Quantity = quantity, UnitPrice = unitPrice };
    }
}
```

#### C. Domain Services
```csharp
public class CalculateOrderTotalService : IDomainService
{
    public decimal CalculateTotal(Order order)
    {
        return order.Lines.Sum(line => line.Quantity * line.UnitPrice);
    }
    
    public decimal CalculateTax(decimal subtotal, decimal taxRate)
    {
        if (taxRate < 0 || taxRate > 1)
            throw new DomainException("Invalid tax rate");
        
        return subtotal * taxRate;
    }
}
```

#### D. Domain Events
```csharp
public class OrderCreatedEvent : DomainEvent
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OrderConfirmedEvent : DomainEvent
{
    public int OrderId { get; set; }
}
```

### Responsabilidades Específicas
- ✅ Definir entidades y value objects
- ✅ Implementar reglas de negocio en métodos
- ✅ Mantener invariantes del aggregate
- ✅ Emitir domain events
- ✅ Validar transiciones de estado
- ✅ NADA de persistencia, NADA de infraestructura

### Responsabilidades PROHIBIDAS
- ❌ Queries a BD
- ❌ Guardar en caché
- ❌ Llamar APIs externas
- ❌ Logging a archivos
- ❌ Uso de HttpClient
- ❌ Cualquier I/O

### Dependencias Típicas
```csharp
public class Order : AggregateRoot
{
    // NUNCA depender de:
    // - IRepository, DbContext, DbSet
    // - IHttpClientFactory
    // - ICache, IDistributedCache
    // - ILogger, IEventBus
    
    // Solo depender de:
    // - ValueObjects (OrderNumber, OrderLine)
    // - Domain Services (CalculateOrderTotalService)
    // - Otras Entities/AggregateRoots
}
```

---

## 4️⃣ Data Access Layer (Persistence)

### Responsabilidad
**Abstraer acceso a datos. Soportar EF Core y Dapper.**

### Proyectos del Framework
- `CompanyName.EntityFrameworkCore` — EF Core abstractions
- `CompanyName.EntityFrameworkCore.SqlServer` — SQL Server implementation
- `CompanyName.Dapper` — Dapper abstractions
- `CompanyName.Dapper.SqlServer` — SQL Server adapter
- `CompanyName.Uow` — Unit of Work pattern

### Componentes Principales

#### A. Repositories (with Specification Pattern)
```csharp
public interface IOrderRepository : IRepository<Order>
{
    // Heredar IRepository proporciona:
    // Task<Order> FirstOrDefaultAsync(ISpecification<Order> spec);
    // Task<List<Order>> ListAsync(ISpecification<Order> spec);
    // Task AddAsync(Order entity);
    // Task UpdateAsync(Order entity);
    // Task DeleteAsync(Order entity);
    
    // Métodos custom si es necesario
    Task<List<Order>> GetByCustomerAsync(int customerId);
}

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(MyDbContext context) : base(context) { }
    
    public async Task<List<Order>> GetByCustomerAsync(int customerId)
    {
        var spec = new GetOrdersByCustomerSpecification(customerId);
        return await ListAsync(spec);
    }
}
```

#### B. Specifications (Query pattern)
```csharp
public class GetOrderByIdSpecification : Specification<Order>
{
    public GetOrderByIdSpecification(int orderId)
    {
        Query
            .Where(o => o.Id == orderId)
            .Include(o => o.Lines)
            .OrderBy(o => o.CreatedAt);
    }
}

public class GetPendingOrdersSpecification : Specification<Order>
{
    public GetPendingOrdersSpecification(int skip = 0, int take = 10)
    {
        Query
            .Where(o => o.Status == OrderStatus.New)
            .Include(o => o.Lines)
            .OrderByDescending(o => o.CreatedAt)
            .Skip(skip)
            .Take(take);
    }
}

// Uso:
var spec = new GetOrderByIdSpecification(orderId);
var order = await _repository.FirstOrDefaultAsync(spec);
```

#### C. DbContext (EF Core)
```csharp
public class MyDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new OrderConfiguration());
        builder.ApplyConfiguration(new OrderLineConfiguration());
        
        // Automatic auditing
        var auditor = new AuditPropertySetter();
        auditor.SetAuditProperties(builder);
    }
}

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Number).IsRequired();
        builder.HasMany(o => o.Lines).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}
```

#### C. Dapper Queries (performance-critical)
```csharp
public class GetOrdersByStatusDapperQuery : IDapperQuery<List<OrderDto>>
{
    private readonly IDbConnection _connection;
    
    public async Task<List<OrderDto>> ExecuteAsync(OrderStatus status)
    {
        var sql = @"
            SELECT o.Id, o.Number, COUNT(*) as LineCount
            FROM Orders o
            LEFT JOIN OrderLines ol ON o.Id = ol.OrderId
            WHERE o.Status = @Status
            GROUP BY o.Id, o.Number
        ";
        
        return (await _connection.QueryAsync<OrderDto>(sql, new { Status = status }))
            .ToList();
    }
}
```

#### D. Unit of Work Pattern
```csharp
[UnitOfWork]  // ← Automatic transaction + event publishing
public async Task<Result> Handle(CreateOrderCommand cmd)
{
    var order = Order.Create(...);
    await _orderRepository.AddAsync(order);
    
    // Automáticamente:
    // 1. Inicia transacción
    // 2. Ejecuta SaveChanges()
    // 3. Publica domain events
    // 4. Commit transacción
    // Si error: Rollback
}
```

### Responsabilidades Específicas
- ✅ Traducir Domain objects ↔ DB records
- ✅ Implementar Specifications
- ✅ Gestionar DbContext
- ✅ Eager loading (Include, ThenInclude)
- ✅ Queries complejas vía Dapper
- ✅ Migrations
- ✅ Unit of Work + Transacciones

### Responsabilidades PROHIBIDAS
- ❌ Lógica de negocio (va al Domain)
- ❌ Orquestación de use cases (va a Application)
- ❌ HTTP concerns (va a Presentation)
- ❌ Caché (va a Infrastructure)

### Dependencias Típicas
```csharp
public class OrderRepository : Repository<Order>
{
    // ✅ Depender de: DbContext, Specifications
    private readonly MyDbContext _context;
}
```

---

## 5️⃣ Infrastructure Layer (Cross-Cutting)

### Responsabilidad
**Implementar concerns transversales. Cache, Events, Security, Auditing, Logging.**

### Proyectos del Framework
- `CompanyName.Autofac` — DI container
- `CompanyName.Caching` + `CompanyName.Caching.StackExchangeRedis` — Cache
- `CompanyName.EventBus` + `CompanyName.EventBus.Azure` — Event bus
- `CompanyName.ServiceBus` + `CompanyName.ServiceBus.Azure` — Message queues
- `CompanyName.Auditing` — Change tracking
- `CompanyName.Security` — Authorization
- `CompanyName.ApplicationInsights.AspNetCore` — Logging

### Responsabilidades Específicas
- ✅ Caché distribuida (Redis)
- ✅ Event Bus (local + Azure)
- ✅ Message queues
- ✅ Auditoría automática
- ✅ Structured logging
- ✅ Security/Claims management
- ✅ Localization resources

---

## 6️⃣ Core Layer (Utilities & Base)

### Responsabilidad
**Utilidades fundamentales, extensiones, marker interfaces.**

### Proyectos del Framework
- `CompanyName.Core` — Base utilities
- `CompanyName.TestBase` — Test helpers

### Componentes
- `ITransientService`, `IScopedService`, `ISingletonService` — Marker interfaces
- Extension methods (LINQ, Reflection, String)
- Expression combiners para Specifications
- Soft delete patterns
- Test builders and fixtures

---

## 🔄 Flujo de una Solicitud (Request Flow)

```
HTTP Request
    ↓
[Presentation Layer]
    ├─ Deserialize body → DTO
    ├─ Route to Endpoint/Controller
    └─ Call Application Service

[Application Layer]
    ├─ Validate input (Validators)
    ├─ Translate DTO → Command
    ├─ Call Domain / Repositories
    └─ Translate response → DTO

[Domain Layer]
    ├─ Validate business rules
    ├─ Execute use case logic
    ├─ Emit domain events
    └─ Return entity

[Data Access Layer]
    ├─ Load via Repository + Specification
    ├─ Save using DbContext / Dapper
    ├─ Transaction management (UoW)
    └─ Return persisted entity

[Infrastructure Layer]
    ├─ Intercept for auditing
    ├─ Publish events to EventBus
    ├─ Store in cache
    ├─ Log to Application Insights
    └─ Apply security checks

[Presentation Layer]
    ├─ Serialize DTO → JSON
    ├─ Set HTTP status code
    └─ Return HTTP Response
```

---

## ✅ Checklist: Estoy colocando mi código en la capa correcta?

### Presentation Layer
- [ ] Solo manejo HTTP (deserialization, serialization, status codes)
- [ ] No tengo lógica de negocio
- [ ] Inyecto Application Services, no Repositories

### Application Layer
- [ ] Orquesto use cases, no implemento lógica
- [ ] Traduzco DTOs ↔ Domain objects
- [ ] Inyecto Repositories, Specifications, Services

### Domain Layer
- [ ] Implemento reglas de negocio
- [ ] CERO dependencias externas
- [ ] No importo ningún paquete de infraestructura
- [ ] Valido invariantes de aggregate

### Data Access Layer
- [ ] Implemento Repositories y Specifications
- [ ] Mapeo Domain objects ↔ DB records
- [ ] Manejo transacciones y migrations

### Infrastructure Layer
- [ ] Implemento concerns transversales
- [ ] Cache, Events, Auditing, Logging
- [ ] Seguridad y autorización

---

## 🔗 Próximos Documentos

1. **04 - Patrones DDD** → Implementación detallada de Aggregates, Value Objects, Services

---

**Versión**: v1.0.0-rc | **Fecha**: 2026-05-13 | **Anterior**: [02 - Principios Arquitectónicos](02-principios-arquitectonicos.md) | **Próximo**: [04 - Patrones DDD](04-patrones-ddd.md)
