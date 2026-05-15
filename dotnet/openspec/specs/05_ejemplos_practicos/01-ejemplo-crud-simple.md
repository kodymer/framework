# 01 - CRUD Simple con DDD

**Sección**: Ejemplos Prácticos | **Documento**: 01 de 04 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Mostrar un ejemplo básico de un CRUD usando DDD, Repository, Unit of Work y un endpoint HTTP.

### Casos de uso
- crear una entidad de dominio
- exponer un servicio REST
- persistir con EF Core
- usar un Application Service

---

## 2. Dominio

### Entidad `Order`
```csharp
public class Order : AggregateRoot<int>
{
    public string OrderNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Order() { }

    public static Order Create(string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new DomainException("OrderNumber is required.");

        return new Order
        {
            OrderNumber = orderNumber,
            CreatedAt = DateTime.UtcNow
        };
    }
}
```

---

## 3. Repositorio y Spec

### Interfaz
```csharp
public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByOrderNumberAsync(string orderNumber);
}
```

### Implementación con Specification
```csharp
public class OrderByOrderNumberSpecification : Specification<Order>
{
    public OrderByOrderNumberSpecification(string orderNumber)
    {
        Query.Where(o => o.OrderNumber == orderNumber);
    }
}

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(MyDbContext context) : base(context) { }

    public Task<Order?> GetByOrderNumberAsync(string orderNumber)
    {
        return FirstOrDefaultAsync(new OrderByOrderNumberSpecification(orderNumber));
    }
}
```

---

## 4. Application Service

### Command
```csharp
public record CreateOrderCommand(string OrderNumber) : ICommand;
```

### Service
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
        var order = Order.Create(command.OrderNumber);
        await _orderRepository.AddAsync(order);

        return Ok(new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            CreatedAt = order.CreatedAt
        });
    }
}
```

### DTO
```csharp
public class OrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

---

## 5. Endpoint

### Minimal API
```csharp
app.MapPost("/orders", async (CreateOrderCommand command, CreateOrderService service) =>
{
    var result = await service.Handle(command);
    return result.IsSuccess
        ? Results.Created($"/orders/{result.Value.Id}", result.Value)
        : Results.BadRequest(result.Errors);
});
```

### Controller
```csharp
[ApiController]
[Route("api/orders")]
public class OrdersController : CompanyNameController
{
    private readonly CreateOrderService _service;

    public OrdersController(CreateOrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderCommand command)
    {
        var result = await _service.Handle(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }
}
```

---

## 6. Testing

### Unit test
```csharp
[Fact]
public async Task CreateOrder_ReturnsSuccess()
{
    var repoMock = new Mock<IOrderRepository>();
    repoMock.Setup(r => r.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

    var service = new CreateOrderService(repoMock.Object);
    var result = await service.Handle(new CreateOrderCommand("ORD-123"));

    Assert.True(result.IsSuccess);
    repoMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
}
```

---

## 7. Checklist

- [ ] Order entity creada
- [ ] Order repository definida
- [ ] CreateOrderService implementado
- [ ] Endpoint expuesto
- [ ] Test unitario validado
