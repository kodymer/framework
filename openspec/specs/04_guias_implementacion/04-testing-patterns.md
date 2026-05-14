# 04 - Testing Patterns

**Sección**: Guías de Implementación | **Documento**: 04 de 04 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Describir los patrones de testing recomendados en el framework para garantizar calidad y confianza.

### Casos de uso
- tests unitarios de dominio
- tests de integración de repositorios
- tests de endpoints HTTP
- mocks para módulos transversales

---

## 2. Unit Tests

### 2.1 Domain Services
- prueba reglas de negocio aisladas
- usa objetos del dominio y value objects

#### Ejemplo
```csharp
[Fact]
public void AddLine_Throws_WhenQuantityIsZero()
{
    var order = Order.Create(OrderNumber.From("ORD-000001"), new List<OrderLine> { OrderLine.Create(1, 1, 10m) });

    var exception = Assert.Throws<DomainException>(() => order.AddLine(OrderLine.Create(2, 0, 5m)));
    Assert.Equal("Line quantity must be positive", exception.Message);
}
```

### 2.2 Application Services
- inyecta mocks para repositorios y buses
- prueba orquestación y casos de éxito/fallo

#### Ejemplo
```csharp
[Fact]
public async Task Handle_ReturnsSuccess_WhenCommandValid()
{
    var orderRepo = new Mock<IOrderRepository>();
    orderRepo.Setup(r => r.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

    var service = new CreateOrderService(orderRepo.Object);
    var result = await service.Handle(new CreateOrderCommand("ORD-000001", ...));

    Assert.True(result.IsSuccess);
    orderRepo.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
}
```

### 2.3 Validators
- prueba reglas de FluentValidation directamente

#### Ejemplo
```csharp
[Fact]
public void CreateOrderValidator_Fails_WhenNumberEmpty()
{
    var validator = new CreateOrderValidator();
    var result = validator.Validate(new CreateOrderCommand(string.Empty, new List<OrderLineCmd>()));

    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.PropertyName == "OrderNumber");
}
```

---

## 3. Integration Tests

### 3.1 Repositories con InMemory
- usa `UseInMemoryDatabase` para EF Core
- valida consultas con specifications

#### Ejemplo
```csharp
[Fact]
public async Task OrderRepository_AddAndGetByNumber()
{
    var options = new DbContextOptionsBuilder<MyDbContext>()
        .UseInMemoryDatabase("OrderRepoTest")
        .Options;

    await using var context = new MyDbContext(options);
    var repository = new OrderRepository(context);

    var order = Order.Create(OrderNumber.From("ORD-000001"), new List<OrderLine> { OrderLine.Create(1, 1, 10m) });
    await repository.AddAsync(order);

    var fetched = await repository.GetByNumberAsync("ORD-000001");
    Assert.NotNull(fetched);
}
```

### 3.2 EventBus integration
- utiliza `EventBusLocal` en tests
- verifica que los handlers sean invocados

#### Ejemplo
```csharp
[Fact]
public async Task EventBusLocal_PublishesDomainEvent()
{
    var eventBus = new LocalEventBus();
    var handler = new Mock<IEventHandler<OrderCreatedEvent>>();
    eventBus.Subscribe(handler.Object);

    await eventBus.PublishAsync(new OrderCreatedEvent(1, "ORD-000001"));

    handler.Verify(h => h.Handle(It.IsAny<OrderCreatedEvent>()), Times.Once);
}
```

---

## 4. End-to-End Tests

### 4.1 API Endpoints
- usa `WebApplicationFactory<T>` para integration tests de ASP.NET Core
- verifica respuestas HTTP y esquemas de JSON

#### Ejemplo
```csharp
[Fact]
public async Task CreateOrder_ReturnsCreated()
{
    await using var factory = new WebApplicationFactory<Program>();
    var client = factory.CreateClient();

    var response = await client.PostAsJsonAsync("/api/orders", new { Number = "ORD-000001" });

    response.StatusCode.Should().Be(HttpStatusCode.Created);
}
```

### 4.2 Frontend / API workflow
- utiliza E2E con Cypress en el template Vue.js
- prueba que la UI consuma correctamente la API

---

## 5. Mocks y Fakes por módulo

### 5.1 Caching
- mockear `ICacheProvider`
- verificar `GetAsync` / `SetAsync`

### 5.2 EventBus
- mockear `IEventBus`
- validar `PublishAsync` llamadas

### 5.3 Security
- mockear `ICurrentUser`
- simular claims

---

## 6. Mejores prácticas

- escribe tests pequeños y deterministas
- evita bases de datos reales en unit tests
- separa tests unitarios de integración
- mantén datos de prueba claros y reutilizables
- usa `TestBuilders` para fixtures complejos

---

## 7. Checklist de testing

- [ ] Coverage de Domain Services
- [ ] Coverage de Application Services
- [ ] Coverage de Repository queries
- [ ] Coverage de Endpoints HTTP
- [ ] Tests de integración para EventBusLocal
- [ ] Tests de validadores
- [ ] Tests de autorización básicos
