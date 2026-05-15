# 4. Patrones DDD (Domain-Driven Design) - CompanyName Framework

**Sección**: Visión y Arquitectura | **Documento**: 04 de 04 | **Estado**: v1.0.0-rc

---

## 🎯 Introduction

Domain-Driven Design (DDD) es una filosofía y un conjunto de patrones para modelar software complejo. El CompanyName Framework implementa los patrones tácticos de DDD:

- **Aggregate Roots** — Raíces que mantienen consistencia
- **Value Objects** — Objetos sin identidad, enfocados en valor
- **Domain Services** — Lógica que cruza múltiples agregates
- **Domain Events** — Cambios significativos en el dominio
- **Repositories** — Abstracción de persistencia
- **Specifications** — Queries type-safe

---

## 1️⃣ Aggregate Roots (Raíces de Agregación)

### ¿Qué es?

Un **Aggregate Root** es una entidad que actúa como boundary (límite) de una agregación. Es responsable de:
- Mantener invariantes (reglas que siempre deben ser verdaderas)
- Controlar acceso a sus objetos internos
- Publicar domain events

### Patrones Clave

#### A. Crear Factory Method (No constructor público)
```csharp
public class Order : AggregateRoot<int>
{
    public OrderNumber Number { get; private set; }  // ← private set
    public OrderStatus Status { get; private set; }
    public List<OrderLine> Lines { get; private set; }
    
    // ❌ No permitir: new Order()
    // ✅ Permitir: Order.Create(...)
    
    public static Order Create(OrderNumber number, List<OrderLine> lines)
    {
        // INVARIANTE 1: Al menos una línea
        if (lines == null || lines.Count == 0)
            throw new DomainException("Order must have at least one line");
        
        var order = new Order
        {
            Number = number,
            Status = OrderStatus.New,
            Lines = lines
        };
        
        // Emitir evento: Order fue creada
        order.AddDomainEvent(new OrderCreatedEvent(order.Id));
        
        return order;  // ← Estado válido garantizado
    }
    
    // Constructor privado (solo para EF Core)
    private Order() { }
}
```

#### B. Métodos que modifican estado (Comportamiento)
```csharp
public class Order : AggregateRoot<int>
{
    // INVARIANTE: Solo se pueden añadir líneas a órdenes NEW
    public void AddLine(OrderLine line)
    {
        if (this.Status != OrderStatus.New)
            throw new DomainException("Cannot add lines to non-new orders");
        
        if (line.Quantity <= 0)
            throw new DomainException("Quantity must be positive");
        
        this.Lines.Add(line);
        this.AddDomainEvent(new OrderLineAddedEvent(this.Id, line.ProductId));
    }
    
    // INVARIANTE: Solo órdenes NEW pueden confirmarse
    public void Confirm()
    {
        if (this.Status != OrderStatus.New)
            throw new DomainException("Order already confirmed");
        
        if (this.Lines.Any(l => l.Quantity > 1000))
            throw new DomainException("Line quantity exceeds maximum");
        
        this.Status = OrderStatus.Confirmed;
        this.AddDomainEvent(new OrderConfirmedEvent(this.Id));
    }
    
    // INVARIANTE: No se puede cancelar una orden ya entregada
    public void Cancel(string reason)
    {
        if (this.Status == OrderStatus.Delivered)
            throw new DomainException("Cannot cancel delivered orders");
        
        this.Status = OrderStatus.Cancelled;
        this.AddDomainEvent(new OrderCancelledEvent(this.Id, reason));
    }
}
```

#### C. Boundary: Acceso restringido a entidades internas
```csharp
public class Order : AggregateRoot<int>
{
    // ✅ Exponer lista como read-only
    private List<OrderLine> _lines = new();
    public IReadOnlyList<OrderLine> Lines => _lines.AsReadOnly();
    
    // ❌ NO hagas:
    // public List<OrderLine> Lines { get; set; }  // Permite cambios sin validación
    
    // Métodos controlados para modificar
    public void AddLine(OrderLine line)
    {
        // Validación aquí
        _lines.Add(line);
    }
    
    public void RemoveLine(int productId)
    {
        var line = _lines.FirstOrDefault(l => l.ProductId == productId);
        if (line == null)
            throw new DomainException("Line not found");
        
        _lines.Remove(line);
    }
}
```

### Cuándo usarlo
- Cuando necesitas garantizar reglas consistentes
- Cuando cambios deben ser atómicos
- Cuando hay jerarquía de objetos que evolucionan juntos

### Ejemplo: Order Aggregate
```
Order (AggregateRoot)
├── OrderNumber (ValueObject)
├── List<OrderLine> (Entities)
│   ├── ProductId
│   ├── Quantity
│   └── UnitPrice
├── OrderStatus (Enum)
└── CreatedAt (DateTime)
```

---

## 2️⃣ Value Objects (Objetos de Valor)

### ¿Qué es?

Un **Value Object** es un objeto que:
- NO tiene identidad única (no tiene ID)
- Es inmutable
- Se compara por sus atributos (no por referencia)
- Encapsula validación

### Patrones Clave

#### A. Definición Básica
```csharp
public class OrderNumber : ValueObject
{
    public string Value { get; }
    
    // Factory method: Garantiza validación
    public static OrderNumber From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("OrderNumber cannot be empty");
        
        if (value.Length > 50)
            throw new DomainException("OrderNumber too long");
        
        if (!Regex.IsMatch(value, @"^ORD-\d{6}$"))
            throw new DomainException("OrderNumber format invalid");
        
        return new OrderNumber(value);
    }
    
    // Constructor privado: Solo vía Factory
    private OrderNumber(string value) => Value = value;
    
    // Comparación por valor
    public override bool Equals(object obj) =>
        obj is OrderNumber on && on.Value == this.Value;
    
    public override int GetHashCode() =>
        Value.GetHashCode();
}

// Uso:
var orderNumber = OrderNumber.From("ORD-000001");  // ✅ Válido
var orderNumber2 = OrderNumber.From("INVALID");    // ❌ DomainException
```

#### B. ValueObject con múltiples atributos
```csharp
public class Money : ValueObject
{
    public decimal Amount { get; }
    public Currency Currency { get; }
    
    public static Money From(decimal amount, Currency currency = Currency.EUR)
    {
        if (amount < 0)
            throw new DomainException("Amount cannot be negative");
        
        return new Money { Amount = amount, Currency = currency };
    }
    
    public Money Add(Money other)
    {
        if (this.Currency != other.Currency)
            throw new DomainException("Cannot add different currencies");
        
        return Money.From(this.Amount + other.Amount, this.Currency);
    }
    
    public override bool Equals(object obj) =>
        obj is Money m && m.Amount == this.Amount && m.Currency == this.Currency;
    
    public override int GetHashCode() =>
        HashCode.Combine(Amount, Currency);
}

// Uso:
var price1 = Money.From(100, Currency.EUR);
var price2 = Money.From(100, Currency.EUR);
Assert.True(price1 == price2);  // True porque se comparan valores

var price3 = Money.From(200, Currency.EUR);
var total = price1.Add(price3);  // = 300 EUR
```

#### C. Colecciones de ValueObjects
```csharp
public class OrderLine : ValueObject
{
    public int ProductId { get; }
    public Quantity Quantity { get; }  // ValueObject
    public Money UnitPrice { get; }     // ValueObject
    
    public static OrderLine Create(int productId, int qty, decimal price)
    {
        if (productId <= 0)
            throw new DomainException("Invalid product");
        
        return new OrderLine
        {
            ProductId = productId,
            Quantity = Quantity.From(qty),
            UnitPrice = Money.From(price)
        };
    }
    
    public Money GetLineTotal() =>
        UnitPrice.Multiply(Quantity.Value);
    
    public override bool Equals(object obj) =>
        obj is OrderLine ol &&
        ol.ProductId == this.ProductId &&
        ol.Quantity == this.Quantity &&
        ol.UnitPrice == this.UnitPrice;
}

// En Aggregate:
public class Order : AggregateRoot<int>
{
    public List<OrderLine> Lines { get; private set; }
    
    public Money GetTotal() =>
        Lines.Aggregate(
            Money.From(0),
            (acc, line) => acc.Add(line.GetLineTotal())
        );
}
```

### Cuándo usarlo
- Conceptos de negocio (OrderNumber, Money, Email)
- Objetos que nunca cambian (inmutables)
- Cuando quieres validación integrada

### Comparativa: Entidad vs Value Object
```csharp
// ❌ INCORRECTO: Address como Entidad
public class Address : Entity<int>  // ← Tiene ID
{
    public int Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
}

// ✅ CORRECTO: Address como Value Object
public class Address : ValueObject
{
    public string Street { get; private set; }
    public string City { get; private set; }
    
    public static Address From(string street, string city)
    {
        if (string.IsNullOrEmpty(street))
            throw new DomainException("Street required");
        
        return new Address { Street = street, City = city };
    }
    
    public override bool Equals(object obj) =>
        obj is Address a && 
        a.Street == this.Street && 
        a.City == this.City;
}
```

---

## 3️⃣ Domain Services

### ¿Qué es?

Un **Domain Service** es un servicio sin estado que implementa lógica que:
- No pertenece a una sola entidad
- Cruza múltiples agregates
- Requiere conocimiento del dominio

### Patrones Clave

#### A. Servicio que calcula (Sin persistencia)
```csharp
public class CalculateOrderTaxService : IDomainService
{
    private readonly ITaxRateProvider _taxRates;
    
    public CalculateOrderTaxService(ITaxRateProvider taxRates)
    {
        _taxRates = taxRates;
    }
    
    public Money CalculateTax(Order order)
    {
        var subtotal = order.GetTotal();
        var rate = _taxRates.GetRateForCountry(order.BillingAddress.Country);
        
        return Money.From(
            subtotal.Amount * (decimal)rate,
            subtotal.Currency
        );
    }
}

// Uso:
var taxService = new CalculateOrderTaxService(taxProvider);
var tax = taxService.CalculateTax(myOrder);
```

#### B. Servicio que coordina múltiples agregates
```csharp
public class CreateShipmentService : IDomainService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IInventoryRepository _inventoryRepo;
    private readonly IShippingProvider _shippingProvider;
    
    public async Task CreateShipmentAsync(int orderId)
    {
        // 1. Cargar orden
        var order = await _orderRepo.GetByIdAsync(orderId);
        
        // 2. Verificar inventario
        foreach (var line in order.Lines)
        {
            var stock = await _inventoryRepo.GetStockAsync(line.ProductId);
            if (stock.AvailableQuantity < line.Quantity.Value)
                throw new DomainException($"Insufficient stock for product {line.ProductId}");
        }
        
        // 3. Crear envío en proveedor
        var shipment = await _shippingProvider.CreateShipmentAsync(
            order.ShippingAddress,
            order.Lines
        );
        
        // 4. Marcar orden como enviada
        order.MarkAsShipped(shipment.TrackingNumber);
        await _orderRepo.UpdateAsync(order);
        
        // 5. Actualizar inventario
        foreach (var line in order.Lines)
        {
            await _inventoryRepo.DecrementStockAsync(line.ProductId, line.Quantity.Value);
        }
    }
}

// Nota: Este servicio COORDINA pero NO implementa reglas de negocio.
// Las reglas están en los Agregates.
```

#### C. Estrategia vs Algoritmo
```csharp
// Domain Service que usa estrategias
public class ApplyDiscountService : IDomainService
{
    private readonly IDiscountStrategy _strategy;
    
    public ApplyDiscountService(IDiscountStrategy strategy)
    {
        _strategy = strategy;  // ← Inyectable
    }
    
    public Money CalculateDiscount(Order order)
    {
        return _strategy.CalculateDiscount(order);
    }
}

// Diferentes estrategias
public interface IDiscountStrategy
{
    Money CalculateDiscount(Order order);
}

public class LoyalCustomerDiscount : IDiscountStrategy
{
    public Money CalculateDiscount(Order order)
    {
        if (order.Customer.IsLoyalForYears(5))
            return Money.From(order.GetTotal().Amount * 0.10m);
        
        return Money.From(0);
    }
}

public class BulkOrderDiscount : IDiscountStrategy
{
    public Money CalculateDiscount(Order order)
    {
        if (order.Lines.Count >= 10)
            return Money.From(order.GetTotal().Amount * 0.05m);
        
        return Money.From(0);
    }
}
```

### Cuándo usarlo
- Lógica que NO pertenece a una entidad
- Coordinar múltiples agregates
- Algoritmos del dominio
- Encapsular "cómo" del dominio

---

## 4️⃣ Domain Events

### ¿Qué es?

Un **Domain Event** es un evento que representa algo significativo que sucedió en el dominio. Otros componentes pueden reaccionar a estos eventos.

### Patrones Clave

#### A. Definir un evento
```csharp
public class OrderCreatedEvent : DomainEvent
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public OrderCreatedEvent(int orderId, string orderNumber)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        CreatedAt = DateTime.UtcNow;
    }
}

public class OrderConfirmedEvent : DomainEvent
{
    public int OrderId { get; set; }
    
    public OrderConfirmedEvent(int orderId)
    {
        OrderId = orderId;
    }
}
```

#### B. Emitir eventos desde Aggregate
```csharp
public class Order : AggregateRoot<int>
{
    public static Order Create(OrderNumber number, List<OrderLine> lines)
    {
        var order = new Order { Number = number, Lines = lines };
        
        // Emitir evento
        order.AddDomainEvent(new OrderCreatedEvent(order.Id, order.Number.Value));
        
        return order;
    }
    
    public void Confirm()
    {
        this.Status = OrderStatus.Confirmed;
        
        // Emitir evento
        this.AddDomainEvent(new OrderConfirmedEvent(this.Id));
    }
}

// Base class proporciona AddDomainEvent():
public abstract class AggregateRoot<TId>
{
    private List<DomainEvent> _events = new();
    
    protected void AddDomainEvent(DomainEvent evt) =>
        _events.Add(evt);
    
    public IReadOnlyList<DomainEvent> GetDomainEvents() =>
        _events.AsReadOnly();
    
    public void ClearDomainEvents() =>
        _events.Clear();
}
```

#### C. Manejar eventos
```csharp
// Handler: Reacciona a evento
public class SendOrderConfirmationEmailHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly IEmailService _emailService;
    
    public SendOrderConfirmationEmailHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }
    
    public async Task Handle(OrderCreatedEvent evt)
    {
        await _emailService.SendAsync(
            to: evt.CustomerEmail,
            subject: "Order Confirmation",
            body: $"Order {evt.OrderNumber} created"
        );
    }
}

// Auto-registrarse en DI
public class IoCEventHandlerFactory
{
    public IEventHandler<TEvent> Create<TEvent>(Type handlerType)
        where TEvent : DomainEvent
    {
        return (IEventHandler<TEvent>)_serviceProvider.GetRequiredService(handlerType);
    }
}
```

#### D. Publicar eventos en Application Service
```csharp
public class CreateOrderService : ApplicationService
{
    [UnitOfWork]
    public async Task<Result<OrderDto>> Handle(CreateOrderCommand cmd)
    {
        var order = Order.Create(cmd.OrderNumber, cmd.Items);
        await _repository.AddAsync(order);
        
        // Publicar eventos: Unit of Work automáticamente hace esto
        await _eventBus.PublishAsync(order.GetDomainEvents());
        
        return Ok(MapToDto(order));
    }
}

// Unit of Work (base class) maneja la publicación
public class UnitOfWorkInterceptor
{
    public async Task Handle(object input, MethodBase method)
    {
        using (var transaction = await BeginTransactionAsync())
        {
            await ProceedAsync();
            
            // Publicar eventos
            var events = await CollectDomainEventsAsync();
            await _eventBus.PublishAsync(events);
            
            await transaction.CommitAsync();
        }
    }
}
```

### Cuándo usarlo
- Cambios significativos en el dominio
- Desacoplar componentes
- Crear auditoría automática
- Acionar procesos asynchronous

---

## 5️⃣ Repositories (Patrón)

### ¿Qué es?

Un **Repository** proporciona una ilusión de colección en memoria para agregates. Abstrae persistencia.

### Patrones Clave

```csharp
// Interface: Lo que necesita el dominio
public interface IOrderRepository : IRepository<Order>
{
    // De IRepository<Order>:
    Task<Order> FirstOrDefaultAsync(ISpecification<Order> spec);
    Task<List<Order>> ListAsync(ISpecification<Order> spec);
    Task AddAsync(Order entity);
    Task UpdateAsync(Order entity);
    Task DeleteAsync(Order entity);
}

// Implementación: Cómo persistir
public class EfCoreOrderRepository : Repository<Order>, IOrderRepository
{
    private readonly MyDbContext _context;
    
    public EfCoreOrderRepository(MyDbContext context) : base(context)
    {
        _context = context;
    }
}

// Uso en Dominio (no ve BD)
public class CreateOrderService
{
    public async Task Handle(CreateOrderCommand cmd)
    {
        var order = Order.Create(...);
        await _orderRepository.AddAsync(order);
        // La BD está abstraída
    }
}
```

---

## 6️⃣ Specifications (Patrón)

### ¿Qué es?

Una **Specification** encapsula una query en un objeto type-safe.

```csharp
// Define query una vez
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

// Usa en muchos lugares
var spec = new GetOrderByIdSpecification(123);
var order = await _repository.FirstOrDefaultAsync(spec);

// Composable
var spec2 = new Specification<Order>()
    .Where(o => o.Status == OrderStatus.New)
    .Include(o => o.Customer)
    .OrderByDescending(o => o.CreatedAt);

var newOrders = await _repository.ListAsync(spec2);
```

---

## 📋 Matriz: Cuándo usar cada patrón

| Patrón | Cuándo | Ejemplo |
|--------|--------|---------|
| **Aggregate Root** | Necesitas garantizar consistencia | Order, Invoice, Customer |
| **Value Object** | Concepto de negocio sin identidad | OrderNumber, Money, Address |
| **Domain Service** | Lógica que cruza múltiples agregates | CalculateTaxService, CreateShipmentService |
| **Domain Event** | Cambio significativo en dominio | OrderCreatedEvent, PaymentReceivedEvent |
| **Repository** | Abstracción de persistencia | IOrderRepository, ICustomerRepository |
| **Specification** | Queries type-safe y reutilizables | GetOrderByIdSpecification, GetPendingOrdersSpec |

---

## 🔗 Próximos Documentos

Este cierra la Sección 1. Próxima: [Módulos Transversales](../../02_modulos_transversales/README.md)

---

**Versión**: v1.0.0-rc | **Fecha**: 2026-05-13 | **Anterior**: [03 - Capas de Arquitectura](03-capas-arquitectura.md) | **Próximo**: [Módulos Transversales](../../02_modulos_transversales/README.md)
