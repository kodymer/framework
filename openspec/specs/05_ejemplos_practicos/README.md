# 5️⃣ Ejemplos Prácticos

**Esta sección proporciona código real y ejecutable para casos comunes.**

---

## 📚 Documentos

| Documento | Propósito |
|-----------|----------|
| [01 - CRUD Simple con DDD](01-ejemplo-crud-simple.md) | Crear entidad, servicio, endpoint |
| [02 - Domain Events y Publishing](02-ejemplo-domain-events.md) | Emitir y manejar eventos de dominio |
| [03 - Caching + Redis + Invalidation](03-ejemplo-caching-redis.md) | Cache distribuida con patrón decorator |
| [04 - Transacciones Distribuidas (Saga)](04-ejemplo-distributed-transactions.md) | Múltiples agregates con compensación |

---

## 🎯 Ejemplos por Complejidad

### 🟢 Nivel Principiante
- [01 - CRUD Simple con DDD](01-ejemplo-crud-simple.md)

**Qué aprendes**:
- ✅ Cómo definir una entidad de dominio
- ✅ Cómo crear un Application Service
- ✅ Cómo exponer via HTTP endpoint
- ✅ Cómo persistir con Repository + EF Core
- ✅ Cómo escribir un test simple

### 🟡 Nivel Intermedio
- [02 - Domain Events y Publishing](02-ejemplo-domain-events.md)
- [03 - Caching + Redis + Invalidation](03-ejemplo-caching-redis.md)

**Qué aprendes**:
- ✅ Cómo emitir eventos desde un aggregate
- ✅ Cómo manejar eventos en handlers
- ✅ Cómo usar EventBus (local y Azure)
- ✅ Cómo implementar caché con invalidación
- ✅ Patrón Decorator para cross-cutting concerns

### 🔴 Nivel Avanzado
- [04 - Transacciones Distribuidas (Saga)](04-ejemplo-distributed-transactions.md)

**Qué aprendes**:
- ✅ Cómo orquestar múltiples servicios
- ✅ Patrón Saga para transacciones distribuidas
- ✅ Compensating transactions y rollback
- ✅ Consistency patterns
- ✅ Error handling en transacciones complejas

---

## 📊 Matriz de Ejemplos

| Ejemplo | Dominio | Patrones | Módulos |
|---------|---------|----------|---------|
| CRUD Simple | Order Management | Repository, UoW | EF Core, Validation |
| Domain Events | Inventory | Domain Events, Event Bus | EventBus, Services |
| Caching | Product Catalog | Decorator, Cache | Caching, Redis |
| Saga Pattern | Payment Processing | Saga, Compensation | EventBus, Auditing |

---

## 🚀 Cómo Usar los Ejemplos

### Opción 1: Aprender Leyendo
1. Lee el documento del ejemplo
2. Observa el código
3. Entiende los patrones
4. Adapta a tu caso de uso

### Opción 2: Aprender Haciendo
1. Lee el documento
2. Crea un nuevo módulo: `dotnet new CompanyNameLower-module -n Practice`
3. Copia el código del ejemplo
4. Ejecuta y experimenta

### Opción 3: Aprender Debugging
1. Clona el repositorio de samples
2. Abre en Visual Studio / Rider
3. Coloca breakpoints
4. Ejecuta y observa el flujo

---

## 💡 Patrones Transversales en Ejemplos

Cada ejemplo demuestra múltiples patrones del framework:

### Patrón UoW (Unit of Work)
```csharp
[UnitOfWork]  // Transacción automática
public async Task Handle(CreateOrderCommand command)
{
    // Cambios se persisten automáticamente
}
```

### Patrón Specification
```csharp
var spec = new GetProductByIdSpecification(productId);
var product = await repository.FirstOrDefaultAsync(spec);
```

### Patrón DDD Service Base
```csharp
public class CreateOrderService : ApplicationService
{
    // Heredas logging, validación, auditoría automáticamente
}
```

### Patrón Validation
```csharp
public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.OrderNumber).NotEmpty();
    }
}
```

---

## 🔗 Links Relacionados

- [Módulos Transversales](../02_modulos_transversales/README.md) — Referencia de módulos usados en ejemplos
- [Guías de Implementación](../04_guias_implementacion/README.md) — Paso-a-paso para proyectos
- [Visión y Arquitectura](../01_vision_arquitectura/README.md) — Conceptos de fondo

---

## 📌 Tips para Adaptarlos

1. **Reemplaza nombres**: Cambia "Order" por tu entidad real
2. **Adapta validaciones**: Añade rules específicas de tu negocio
3. **Integra módulos**: Activa caché, auditoría, seguridad según necesites
4. **Escala patrones**: Los mismos patterns funcionan para 100 entidades

---

**Estado**: ✅ Completed (Fase 6)  
**Documentos**: 4 generados  
**Nivel de cobertura**: Principiante → Avanzado
