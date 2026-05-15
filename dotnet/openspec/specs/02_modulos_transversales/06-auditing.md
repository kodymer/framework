# 06 - Auditing

**Sección**: Módulos Transversales | **Documento**: 06 de 10 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Registrar automáticamente cambios de estado en la aplicación para:
- trazabilidad
- cumplimiento
- análisis forense
- auditoría de modificaciones de datos

### Casos de uso
- seguimiento de quién cambió qué y cuándo
- auditoría de accesos a datos sensibles
- reconstrucción de incidentes
- monitorizar modificaciones de entidades críticas

### Stack tecnológico
- `Microsoft.EntityFrameworkCore`
- interceptores de EF Core
- `CompanyName.Auditing`

---

## 2. Abstracciones Principales

### Interfaces clave
- `IAuditPropertySetter`
- `IAuditEntry`
- `IAuditableEntity`

### Clases base
- `AuditDbContextInterceptor`
- `AuditPropertySetter`
- `AuditableEntityBase`

### Namespaces
- `CompanyName.Auditing`
- `CompanyName.Auditing.EntityFramework`

---

## 3. Comportamiento Default

### Implementación out-of-box
- `AuditDbContextInterceptor` captura `SaveChanges`
- `AuditPropertySetter` define `CreatedBy`, `CreatedAt`, `ModifiedBy`, `ModifiedAt`
- `AuditableEntityBase` proporciona campos auditables

### Reglas predeterminadas
- se establecen valores automáticamente en el ciclo de vida de EF Core
- solo las entidades que implementan `IAuditableEntity` son auditadas
- el usuario actual se resuelve desde `ICurrentUser`

---

## 4. Configuración en Program.cs

```csharp
builder.Services.AddScoped<IAuditPropertySetter, AuditPropertySetter>();
builder.Services.AddScoped<AuditDbContextInterceptor>();

builder.Services.AddDbContext<MyDbContext>((sp, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.AddInterceptors(sp.GetRequiredService<AuditDbContextInterceptor>());
});
```

---

## 5. Patrones de Uso

### Inyección de dependencias
```csharp
public class MyDbContext : DbContext
{
    private readonly AuditDbContextInterceptor _auditInterceptor;

    public MyDbContext(DbContextOptions<MyDbContext> options, AuditDbContextInterceptor auditInterceptor)
        : base(options)
    {
        _auditInterceptor = auditInterceptor;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditInterceptor);
    }
}
```

### Uso práctico
```csharp
public class Order : AuditableEntityBase
{
    public int Id { get; set; }
    public string Number { get; set; }
}
```

### Best practices
- auditar solo entidades críticas
- conservar historial de cambios en tablas separadas si se requiere
- usar `ICurrentUser` para atar usuario a cada modificación

---

## 6. Extensión Personalizada

### Interfaces a implementar
- `IAuditPropertySetter`
- `IAuditableEntity`

### Registro personalizado
```csharp
services.AddScoped<IAuditPropertySetter, CustomAuditPropertySetter>();
```

### Ejemplo step-by-step
1. Implementa `IAuditPropertySetter`
2. Asegúrate de que tu `DbContext` registre el interceptor
3. Extiende `AuditableEntityBase` o implementa `IAuditableEntity`
4. Verifica los campos `CreatedBy` / `ModifiedBy`

---

## 7. Testing

### Mock/stub recommendations
- Mock `ICurrentUser`
- Simula `AuditDbContextInterceptor`

### Ejemplo de test
```csharp
[Fact]
public async Task SaveChanges_SetsAuditFields()
{
    var currentUserMock = new Mock<ICurrentUser>();
    currentUserMock.Setup(x => x.Id).Returns("user-1");

    var interceptor = new AuditDbContextInterceptor(currentUserMock.Object);
    var options = new DbContextOptionsBuilder<MyDbContext>()
        .UseInMemoryDatabase("AuditTest")
        .AddInterceptors(interceptor)
        .Options;

    await using var context = new MyDbContext(options);
    context.Orders.Add(new Order { Number = "ORD-001" });
    await context.SaveChangesAsync();

    var order = await context.Orders.FirstAsync();
    Assert.Equal("user-1", order.CreatedBy);
}
```