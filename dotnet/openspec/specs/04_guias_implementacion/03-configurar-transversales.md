# 03 - Configurar Módulos Transversales

**Sección**: Guías de Implementación | **Documento**: 03 de 04 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Explicar cómo habilitar y configurar los principales módulos transversales del framework en un proyecto recién generado.

### Casos de uso
- activar caching Redis
- habilitar EventBus local o Azure
- configurar logging y Application Insights
- registrar validación, security y localization

---

## 2. Configuración general

### Registrar módulos base
En `Program.cs`:
```csharp
builder.Services.AddCaching(options =>
{
    options.UseInMemoryCache = true;
    options.DefaultSlidingExpiration = TimeSpan.FromMinutes(5);
});

builder.Services.AddEventBusLocal();
builder.Services.AddServiceBusAzure(builder.Configuration);
builder.Services.AddBaseServices();
builder.Services.AddRepositoryPattern();
```

### Agregar middleware requerido
```csharp
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestLocalization();
app.UseSwagger();
app.UseSwaggerUI();
```

---

## 3. Configuración de cada módulo

### 3.1 Caching
```csharp
builder.Services.AddCaching(options =>
{
    options.UseInMemoryCache = true;
    options.RedisConfiguration = builder.Configuration.GetConnectionString("Redis");
    options.DefaultSlidingExpiration = TimeSpan.FromMinutes(10);
});
```

### 3.2 EventBus
```csharp
builder.Services.AddEventBusLocal();
```

### 3.3 ServiceBus
```csharp
builder.Services.AddServiceBusAzure(builder.Configuration);
```

### 3.4 Repository Pattern
```csharp
builder.Services.AddRepositoryPattern();
```

### 3.5 Base Services
```csharp
builder.Services.AddBaseServices();
```

### 3.6 Auditing
```csharp
builder.Services.AddScoped<IAuditPropertySetter, AuditPropertySetter>();
builder.Services.AddScoped<AuditDbContextInterceptor>();
```

### 3.7 Security
```csharp
builder.Services.AddScoped<ICurrentUser, CurrentUserAccessor>();
```

### 3.8 Localization
```csharp
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});
```

### 3.9 Logging
```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

### 3.10 Validation
```csharp
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();
```

---

## 4. Ejemplo completo de Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCaching(options =>
{
    options.UseInMemoryCache = true;
    options.DefaultSlidingExpiration = TimeSpan.FromMinutes(10);
});

builder.Services.AddEventBusLocal();
builder.Services.AddServiceBusAzure(builder.Configuration);
builder.Services.AddRepositoryPattern();
builder.Services.AddBaseServices();

builder.Services.AddScoped<IAuditPropertySetter, AuditPropertySetter>();
builder.Services.AddScoped<AuditDbContextInterceptor>();

builder.Services.AddScoped<ICurrentUser, CurrentUserAccessor>();

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();

var app = builder.Build();
app.UseRequestLocalization();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
```

---

## 5. Troubleshooting

### Redis no se conecta
- valida `RedisConfiguration`
- revisa firewall y network
- prueba `redis-cli`

### EventBus no publica
- asegúrate de usar `AddEventBusLocal` o `AddEventBusAzure`
- revisa la configuración de `AzureServiceBus`

### Validación no se ejecuta
- comprueba que `AddValidatorsFromAssemblyContaining` incluya el ensamblado correcto
- revisa si el middleware o pipeline invoca validadores

---

## 6. Tips

- usa `EventBusLocal` en desarrollo y `EventBusAzure` en producción
- mantén `Caching` activado en todos los entornos salvo en tests unitarios
- usa un `CurrentUserAccessor` consistente para auditoría y seguridad
