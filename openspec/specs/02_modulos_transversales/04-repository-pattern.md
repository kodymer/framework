# 04 - Repository Pattern (EF Core + Dapper)

**Sección**: Módulos Transversales | **Documento**: 04 de 10 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Proporcionar una capa de abstracción de persistencia que permita:
- usar EF Core para operaciones CRUD y relaciones ORM
- usar Dapper para consultas SQL de alto rendimiento
- mantener el dominio aislado de la infraestructura de datos

### Casos de uso
- CRUD de agregados
- consultas estructuradas con inclusión de relaciones
- consultas analíticas o de reporting con Dapper
- mantener compatibilidad entre múltiples proveedores de datos

### Stack tecnológico
- `Microsoft.EntityFrameworkCore`
- `Dapper`
- `Ardalis.Specification`
- `CompanyName.Uow`

---

## 2. Abstracciones Principales

### Interfaces clave
- `IRepository<T>`
- `IReadRepository<T>`
- `IWriteRepository<T>`
- `IDapperQuery<T>`

### Clases abstractas
- `Repository<T>`
- `ReadOnlyRepository<T>`
- `DapperQueryBase<T>`

### Namespaces
- `CompanyName.EntityFrameworkCore`
- `CompanyName.Dapper`

---

## 3. Comportamiento Default

### Implementación out-of-box
- `EfCoreRepository<T>` utiliza `DbContext`
- `EfCoreReadOnlyRepository<T>` expone consultas de solo lectura
- `DapperSqlServerQuery<T>` ejecuta SQL raw en `IDbConnection`
- `SpecificationEvaluator` traduce `ISpecification<T>` en consultas EF Core

### Reglas predeterminadas
- usar `Specification` para queries reutilizables
- evitar `DbContext` en la capa de dominio
- usar Dapper solo donde se requiera rendimiento

---

## 4. Configuración en Program.cs

```csharp
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfCoreReadOnlyRepository<>));
```

```csharp
public static class RepositoryServiceCollectionExtensions
{
    public static IServiceCollection AddRepositoryPattern(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EfCoreReadOnlyRepository<>));
        services.AddScoped<IDbConnection>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            return new SqlConnection(config.GetConnectionString("DefaultConnection"));
        });
        return services;
    }
}
```

---

## 5. Patrones de Uso

### Inyección de dependencias
```csharp
public class OrderRepository : EfCoreRepository<Order>, IOrderRepository
{
    public OrderRepository(MyDbContext dbContext) : base(dbContext) { }
}
```

### Uso práctico (Specification)
```csharp
var spec = new GetPendingOrdersSpecification();
var orders = await _orderRepository.ListAsync(spec);
```

### Uso práctico (Dapper)
```csharp
public class GetOrderSummariesQuery : DapperQueryBase<OrderSummaryDto>
{
    public override string Sql => @"
        SELECT Id, Number, Total
        FROM Orders
        WHERE Status = @Status";
}
```

### Best practices
- Usa `Specification` para consultas domain-friendly
- Emplea Dapper solo en consultas de alta complejidad/performance
- No mezcles lógica de negocio en consultas Dapper

---

## 6. Extensión Personalizada

### Interfaces a implementar
- `IRepository<T>` para repositorios custom
- `IDapperQuery<T>` para queries específicas

### Registro personalizado
```csharp
services.AddScoped<IOrderRepository, CustomOrderRepository>();
services.AddScoped<IDapperQuery<OrderSummaryDto>, GetOrderSummariesQuery>();
```

### Ejemplo step-by-step
1. Define `IOrderRepository` extendiendo `IRepository<Order>`
2. Implementa `CustomOrderRepository`
3. Registra la implementación en DI
4. Consume desde Application Service

---

## 7. Testing

### Mock/stub recommendations
- Mock `IRepository<T>` y `IReadRepository<T>`
- Usa `InMemoryDbContext` para pruebas EF Core

### Ejemplo de test
```csharp
[Fact]
public async Task AddOrder_SavesEntity()
{
    var dbContext = new TestDbContext();
    var repository = new OrderRepository(dbContext);

    await repository.AddAsync(new Order());

    Assert.Equal(1, dbContext.Orders.Count());
}
```
