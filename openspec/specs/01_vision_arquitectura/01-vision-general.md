# 1. Visión General - CompanyName Framework

**Sección**: Visión y Arquitectura | **Documento**: 01 de 04 | **Estado**: v1.0.0-rc

---

## 📖 ¿Qué es el CompanyName Framework?

El **CompanyName Framework** es un marco de trabajo empresarial modular y extensible construido sobre **Domain-Driven Design (DDD)**, **CQRS** y **Clean Architecture** para .NET 8.0. 

Proporciona:
- ✅ **Arquitectura probada**: Patrones DDD implementados desde el inicio
- ✅ **Plantillas reutilizables**: Generar módulos, APIs y frontends con `dotnet new`
- ✅ **Módulos transversales pre-configurados**: Caché, eventos, auditoría, seguridad, etc.
- ✅ **Aceleración del desarrollo**: Clases base de servicios, patrón Repository con EF Core + Dapper
- ✅ **Extensibilidad clara**: Interfaces bien definidas, DI con Autofac, markers interfaces
- ✅ **Enterprise-ready**: Auditoría, multi-idioma, Azure integration, logging estructurado

**Objetivo**: Reducir el tiempo de bootstrap de proyectos, mantener consistencia arquitectónica y proporcionar un foundation sólido para aplicaciones complejas.

---

## 🎯 Para Quién Es

### Público Objetivo

| Perfil | Necesidad | Valor |
|--------|----------|-------|
| **Equipos de Verisure** | Estándar arquitectónico corporativo | Consistencia entre proyectos |
| **Desarrolladores .NET** | Framework productivo con patterns claros | Menos boilerplate, más negocio |
| **Arquitectos de Software** | Base modular y extensible | Fácil mantenimiento y evolución |
| **Nuevos Desarrolladores** | Onboarding con ejemplos claros | Curva de aprendizaje reducida |

### No es Para

- ❌ Proyectos CRUD muy simples (usar MVC tradicional)
- ❌ Aplicaciones sin lógica de dominio compleja
- ❌ Startups que necesitan velocidad extrema sobre arquitectura

---

## 💡 Beneficios Principales

### 1. Arquitectura Robusta Desde el Inicio
- DDD implementado con Domain Services y Application Services
- Clean Architecture con capas claras: Presentation → Application → Domain → Data Access
- Patrones probados: Specification, Repository, Unit of Work, Event Sourcing

**Beneficio**: Código mantenible a escala. Proyectos de 100+ funcionalidades sin degradación.

### 2. Desarrollo Acelerado
- Plantillas `dotnet new` que generan estructura completa
- Clases base de servicios que heredan auditoría, logging, validación
- Repository pattern pre-configurado con EF Core + Dapper

**Beneficio**: Un CRUD completo se implementa en horas, no días.

### 3. Consistencia Corporativa
- Estándar único para todos los proyectos Verisure
- Convenciones claras: namespacing, folder structure, naming
- Módulos transversales compartidos (caché, eventos, seguridad)

**Beneficio**: Desarrolladores pueden moverse entre proyectos sin fricción.

### 4. Extensibilidad Sin Modificación
- Módulos transversales pueden reemplazarse (Local ↔ Azure)
- Nuevas funcionalidades vía interfaces + DI
- Patrón Plugin mediante marker interfaces

**Beneficio**: Evolucionar sin romper código existente.

### 5. Preparado para Escala
- Event Bus para comunicación entre servicios
- Caching distribuida (Redis)
- Auditoría automática
- Unit of Work con transacciones distribuidas

**Beneficio**: Crecer de monolito a microservicios sin reescribir.

---

## 📊 Comparativa: Con/Sin Framework

### Sin Framework (Typical MVC Project)

```csharp
// Controllers
public class OrdersController : Controller
{
    [HttpPost]
    public IActionResult CreateOrder(CreateOrderDto dto)
    {
        // ❌ Lógica de negocio en controller
        // ❌ Validación repetida
        // ❌ Sin auditoría automática
        // ❌ BD directamente desde controller
        var order = new Order { ... };
        _db.Orders.Add(order);
        _db.SaveChanges();
        return Ok();
    }
}
```

**Problemas**: Difícil testear, lógica esparcida, sin separación de concerns.

### Con Framework (DDD Approach)

```csharp
// Domain Service (Lógica de negocio)
public class CreateOrderService : ApplicationService
{
    // ✅ Heredas: logging, validación, auditoría, UoW
    // ✅ Injection automática de dependencias
    // ✅ Specification pattern para queries
    
    [UnitOfWork]  // ← Transacción automática
    public async Task<Result<OrderDto>> Handle(CreateOrderCommand cmd)
    {
        // Lógica clara y centrada en negocio
        var order = Order.Create(cmd.OrderNumber, cmd.Items);
        
        // Repository pattern
        await _repository.AddAsync(order);
        
        // Eventos automáticos
        await _eventBus.PublishAsync(
            new OrderCreatedEvent(order.Id)
        );
        
        return Ok(MapToDto(order));
    }
}

// API Endpoint (Minimal)
public class CreateOrderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/orders", Handle);
    
    private async Task<IResult> Handle(
        CreateOrderCommand cmd,
        CreateOrderService service) =>
        (await service.Handle(cmd)).ToIResult();
}
```

**Ventajas**: Código limpio, testeable, auditable, escalable.

---

## 🏗️ Componentes Principales

### 1. Arquitectura en Capas
```
Presentation (ASP.NET Core)
    ↓ HTTP
Application Services (DDD)
    ↓ Business rules
Domain (Entities, Value Objects, Services)
    ↓ Persistence
Data Access (Repository, EF Core, Dapper)
    ↓ Database
Infrastructure (Cache, Events, Security, Auditing)
```

### 2. 10 Módulos Transversales
- **Caching**: Hybrid + Redis
- **EventBus**: Local + Azure
- **ServiceBus**: Local + Azure
- **Repository Pattern**: EF Core + Dapper + Specifications
- **Base Services**: Domain + Application
- **Auditing**: Change tracking automático
- **Security**: Claims-based auth
- **Localization**: Multi-idioma
- **Logging**: Application Insights
- **Validation**: FluentValidation

### 3. 3 Plantillas Concretas
- **aspnetcore-module**: Módulo sin Host
- **aspnetcore-webapi**: API con Host
- **vuejs**: Frontend Vue.js 3

### 4. Stack Tecnológico
- **.NET 8.0** — Runtime
- **EF Core 9.0.11** — ORM
- **Dapper 2.1.66** — Micro-queries
- **Autofac 9.0** — DI container avanzado
- **Azure SDK** — Cloud integration
- **FluentValidation 12.1.1** — Input validation
- **AutoMapper 14.0** — DTO mapping

---

## 🎬 Flujo Típico de Uso

### Day 1: Setup Inicial
```bash
# 1. Instalar paquete de templates
dotnet new -i d:\accelerator\framework\templates\src\CompanyName.Templates

# 2. Generar módulo
dotnet new CompanyNameLower-module -n OrderModule
cd OrderModule

# 3. Generar API
dotnet new CompanyNameLower-webapi -n OrderApi
```

### Day 1-2: Implementación
```csharp
// 1. Define dominio (DDD)
public class Order : AggregateRoot
{
    public OrderNumber Number { get; private set; }
    public List<OrderLine> Lines { get; private set; }
    
    public static Order Create(OrderNumber number, List<OrderLine> lines)
    {
        // Validación de reglas de negocio
        if (lines.Count == 0)
            throw new DomainException("Order must have at least one line");
        
        var order = new Order { Number = number, Lines = lines };
        order.AddDomainEvent(new OrderCreatedEvent(order.Id));
        return order;
    }
}

// 2. Servicio de aplicación (usa base class)
public class CreateOrderService : ApplicationService
{
    [UnitOfWork]
    public async Task<Result<OrderDto>> Handle(CreateOrderCommand cmd)
    {
        var order = Order.Create(
            OrderNumber.From(cmd.OrderNumber),
            cmd.Lines.Select(MapToDomainLine).ToList()
        );
        
        await _orderRepository.AddAsync(order);
        // UoW + auditoría + logging automáticos
        
        return Ok(MapToDto(order));
    }
}

// 3. Endpoint
public class CreateOrderEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/orders", Handle);
    
    private async Task<IResult> Handle(CreateOrderCommand cmd, CreateOrderService svc) =>
        (await svc.Handle(cmd)).ToIResult();
}
```

### Day 3: Integración y Deploy
```csharp
// Configurar módulos transversales
var builder = WebApplication.CreateBuilder(args);

// Base framework
builder.Services
    .AddCompanyNameCore()
    .AddDddDomain()
    .AddApplicationServices()
    .AddEntityFrameworkCore(builder.Configuration)
    .AddRepositories();

// Transversales
builder.Services
    .AddCaching(builder.Configuration)  // Redis
    .AddEventBus(builder.Configuration)  // Azure Service Bus
    .AddAuditing()
    .AddSecurityClaims()
    .AddValidation()
    .AddLogging();

var app = builder.Build();
app.MapEndpoints();
await app.RunAsync();
```

---

## 📈 Expectativas de Productividad

| Fase | Sin Framework | Con Framework | Ganancia |
|------|---------------|--------------|----------|
| **Scaffolding** | 2-3 días | 30 minutos | ⚡ 6x más rápido |
| **CRUD básico** | 3-5 días | 1-2 horas | ⚡ 10x más rápido |
| **Testing** | 3-5 días | 1-2 días | ⚡ 3x más rápido |
| **Módulo completo** | 4-6 semanas | 1-2 semanas | ⚡ 3x más rápido |

---

## ✅ Criterios de Éxito

Considerar el framework exitoso si:

- ✅ Nuevos proyectos se bootstrappean en < 1 hora
- ✅ CRUD de entidad se implementa en < 4 horas
- ✅ Tests cubriran ≥ 80% de lógica de dominio
- ✅ Developers entiendan patrones en 1-2 sprints
- ✅ Cambios entre proyectos <1 día
- ✅ Bugs arquitectónicos < 5% del total

---

## 🔗 Próximos Documentos

1. **02 - Principios Arquitectónicos** → Valores que guían decisiones
2. **03 - Capas de Arquitectura** → Estructura detallada
3. **04 - Patrones DDD** → Cómo se implementa DDD

---

## 📝 Resumen Ejecutivo

| Aspecto | Valor |
|--------|-------|
| **¿Qué?** | Framework DDD/CQRS/Clean Architecture para .NET 8.0 |
| **¿Para quién?** | Equipos Verisure, desarrolladores que valoran arquitectura |
| **¿Por qué?** | Acelerar desarrollo, mantener consistencia, escalar sin reescribir |
| **¿Cómo?** | Plantillas `dotnet new` + módulos pre-configurados + servicios base |
| **¿Cuándo?** | v1.0.0-rc — listo para producción |
| **¿Resultados?** | Proyectos 3-6x más rápidos, código 80%+ testeable |

---

**Versión**: v1.0.0-rc | **Fecha**: 2026-05-13 | **Próximo**: [02 - Principios Arquitectónicos](02-principios-arquitectonicos.md)
