# 02 - Domain Events y Publishing

**Sección**: Ejemplos Prácticos | **Documento**: 02 de 04 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Demostrar cómo emitir eventos desde un aggregate y publicarlos mediante el EventBus para desacoplar acciones posteriores.

### Casos de uso
- notificar otros subsistemas de cambios de estado
- ejecutar acciones asincrónicas luego de un commit
- mantener el dominio limpio de dependencias externas

---

## 2. Emitir domain events

### Evento
```csharp
public class OrderCreatedEvent : DomainEvent
{
    public int OrderId { get; }
    public string OrderNumber { get; }

    public OrderCreatedEvent(int orderId, string orderNumber)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
    }
}
```

### Aggregate
```csharp
public class Order : AggregateRoot<int>
{
    public string OrderNumber { get; private set; }

    public static Order Create(string orderNumber)
    {
        var order = new Order { OrderNumber = orderNumber };
        order.AddDomainEvent(new OrderCreatedEvent(order.Id, orderNumber));
        return order;
    }
}
```

---

## 3. Publicar eventos en UoW

### Application Service
```csharp
public class CreateOrderService : ApplicationService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;

    public CreateOrderService(IOrderRepository orderRepository, IEventBus eventBus)
    {
        _orderRepository = orderRepository;
        _eventBus = eventBus;
    }

    [UnitOfWork]
    public async Task<Result<OrderDto>> Handle(CreateOrderCommand command)
    {
        var order = Order.Create(command.OrderNumber);
        await _orderRepository.AddAsync(order);

        await _eventBus.PublishAsync(order.GetDomainEvents());

        return Ok(new OrderDto { Id = order.Id, OrderNumber = order.OrderNumber });
    }
}
```

---

## 4. Handler de evento

### Implementación
```csharp
public class SendOrderCreatedEmailHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly IEmailService _emailService;

    public SendOrderCreatedEmailHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(OrderCreatedEvent @event)
    {
        await _emailService.SendAsync(
            to: "ops@example.com",
            subject: $"Nueva orden {@event.OrderNumber}",
            body: $"Orden {@event.OrderId} creada"
        );
    }
}
```

### Registro en DI
```csharp
services.Scan(scan => scan
    .FromApplicationDependencies()
    .AddClasses(classes => classes.AssignableTo(typeof(IEventHandler<>)))
    .AsImplementedInterfaces()
    .WithSingletonLifetime());
```

---

## 5. EventBus local vs Azure

### Local
```csharp
builder.Services.AddEventBusLocal();
```

### Azure
```csharp
builder.Services.AddEventBusAzure(builder.Configuration);
```

---

## 6. Testing

### Unit test del handler
```csharp
[Fact]
public async Task Handle_SendsEmail()
{
    var emailMock = new Mock<IEmailService>();
    var handler = new SendOrderCreatedEmailHandler(emailMock.Object);

    await handler.Handle(new OrderCreatedEvent(1, "ORD-123"));

    emailMock.Verify(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
}
```

### Integration test del EventBus
```csharp
[Fact]
public async Task EventBusLocal_InvokesHandler()
{
    var eventBus = new LocalEventBus();
    var handlerMock = new Mock<IEventHandler<OrderCreatedEvent>>();
    eventBus.Subscribe(handlerMock.Object);

    await eventBus.PublishAsync(new OrderCreatedEvent(1, "ORD-123"));

    handlerMock.Verify(h => h.Handle(It.IsAny<OrderCreatedEvent>()), Times.Once);
}
```

---

## 7. Checklist

- [ ] Evento de dominio definido
- [ ] Aggregate emite evento
- [ ] EventBus configurado
- [ ] Handler implementado
- [ ] Test unitario del handler
- [ ] Test de EventBus local
