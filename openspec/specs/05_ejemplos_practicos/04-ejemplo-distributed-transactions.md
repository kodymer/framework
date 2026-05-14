# 04 - Transacciones Distribuidas (Saga)

**Sección**: Ejemplos Prácticos | **Documento**: 04 de 04 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Mostrar un patrón de transacción distribuida con compensación (Saga) para coordinar múltiples servicios y garantizar consistencia eventual.

### Casos de uso
- pago + pedido + inventario
- orquestación entre microservicios
- operaciones que no pueden resolverse en una sola transacción de DB

---

## 2. Patrón Saga

### Definición
Una saga coordina varios pasos que pueden fallar, y ejecuta operaciones de compensación cuando es necesario.

### Componentes
- `ISagaStep`
- `SagaCoordinator`
- comandos de compensación

---

## 3. Ejemplo: Crear orden con pago

### Paso 1: Crear orden
```csharp
public class CreateOrderSaga : ISaga
{
    private readonly IOrderService _orderService;
    private readonly IPaymentService _paymentService;
    private readonly IInventoryService _inventoryService;

    public async Task ExecuteAsync(CreateOrderSagaData data)
    {
        var order = await _orderService.CreateOrderAsync(data.OrderNumber);
        try
        {
            await _paymentService.ChargeAsync(data.PaymentInfo);
            await _inventoryService.ReserveStockAsync(order.Id, data.Items);
        }
        catch
        {
            await _orderService.CancelOrderAsync(order.Id);
            throw;
        }
    }
}
```

---

## 4. Compensaciones

### Cancelar orden si falla pago
```csharp
public async Task ExecuteAsync(CreateOrderSagaData data)
{
    var order = await _orderService.CreateOrderAsync(data.OrderNumber);
    var paymentSucceeded = false;

    try
    {
        await _paymentService.ChargeAsync(data.PaymentInfo);
        paymentSucceeded = true;
        await _inventoryService.ReserveStockAsync(order.Id, data.Items);
    }
    catch
    {
        if (paymentSucceeded)
        {
            await _paymentService.RefundAsync(data.PaymentInfo);
        }

        await _orderService.CancelOrderAsync(order.Id);
        throw;
    }
}
```

---

## 5. Orquestador

### Coordinador simple
```csharp
public class SagaCoordinator
{
    private readonly IEnumerable<ISagaStep> _steps;

    public async Task RunAsync(SagaContext context)
    {
        foreach (var step in _steps)
        {
            await step.ExecuteAsync(context);
        }
    }
}
```

---

## 6. Testing

### Test de compensación
```csharp
[Fact]
public async Task Saga_Compensates_WhenPaymentFails()
{
    var orderServiceMock = new Mock<IOrderService>();
    var paymentServiceMock = new Mock<IPaymentService>();
    var inventoryServiceMock = new Mock<IInventoryService>();

    paymentServiceMock.Setup(x => x.ChargeAsync(It.IsAny<PaymentInfo>())).ThrowsAsync(new Exception("Payment failed"));

    var saga = new CreateOrderSaga(orderServiceMock.Object, paymentServiceMock.Object, inventoryServiceMock.Object);

    await Assert.ThrowsAsync<Exception>(() => saga.ExecuteAsync(new CreateOrderSagaData()));
    orderServiceMock.Verify(x => x.CancelOrderAsync(It.IsAny<int>()), Times.Once);
}
```

---

## 7. Checklist

- [ ] Saga definida
- [ ] Pasos implementados
- [ ] Compensación cubierta
- [ ] Orquestador creado
- [ ] Test de error de compensación
