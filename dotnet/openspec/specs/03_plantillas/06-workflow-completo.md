# 06 - Workflow Completo

**Sección**: Plantillas y Generación de Código | **Documento**: 06 de 06 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Proveer un flujo completo desde la instalación de templates hasta la ejecución inicial de los proyectos generados.

### Casos de uso
- iniciar un nuevo proyecto con las plantillas del framework
- generar backend y frontend coherentes
- integrar módulos de negocio en una API

---

## 2. Flujo Recomendado

### Paso 1: Instalar los templates
```bash
dotnet new -i d:\accelerator\framework\templates\src\CompanyName.Templates
```

### Paso 2: Generar un módulo de negocio
```bash
dotnet new CompanyNameLower-module -n OrderModule
```

### Paso 3: Generar la API principal
```bash
dotnet new CompanyNameLower-webapi -n OrderApi
```

### Paso 4: Generar el frontend
```bash
dotnet new CompanyNameLower-vue -n OrderUI
```

---

## 3. Integrar el módulo en la API

### 1. Agregar referencia
Dentro de `OrderApi.sln`, añade proyecto de módulo:
```bash
dotnet sln add ..\OrderModule\src\OrderModule.Domain\OrderModule.Domain.csproj
```

### 2. Registrar servicios del módulo
En `OrderApi.HttpApi.Host` agrega las dependencias del módulo:
```csharp
services.AddScoped<IOrderRepository, OrderModule.OrderModuleRepository>();
services.AddScoped<CreateOrderService>();
```

### 3. Mapear endpoints
- usa `OrderModule.HttpApi` o crea un endpoint propio en `OrderApi.HttpApi`
- dirije requests a los servicios del módulo

---

## 4. Configurar módulos transversales

### Ejemplo en `Program.cs`
```csharp
builder.Services.AddCaching(options =>
{
    options.UseInMemoryCache = true;
    options.DefaultSlidingExpiration = TimeSpan.FromMinutes(10);
});

builder.Services.AddEventBusLocal();
builder.Services.AddServiceBusAzure(builder.Configuration);
builder.Services.AddBaseServices();
builder.Services.AddRepositoryPattern();
```

### Seguridad y observabilidad
```csharp
builder.Services.AddAuthentication("Bearer").AddJwtBearer(...);
builder.Services.AddAuthorization();
builder.Services.AddApplicationInsightsTelemetry();
```

---

## 5. Ejecutar todo localmente

### Backend
```bash
cd OrderApi/src/OrderApi.HttpApi.Host
dotnet run
```

### Frontend
```bash
cd OrderUI
npm install
npm run dev
```

---

## 6. Validar integración

### Pruebas básicas
- acceder a `http://localhost:5000/swagger`
- verificar endpoints expuestos
- consumir API desde el frontend

### Tests
```bash
dotnet test OrderApi.sln
npm run test:unit
npm run test:e2e
```

---

## 7. Deploy básico

### Publish Web API
```bash
dotnet publish OrderApi/src/OrderApi.HttpApi.Host -c Release -o ./publish
```

### Build frontend
```bash
cd OrderUI
npm run build
```

---

## 8. Consejos finales

- usa `OrderModule` para encapsular dominio y lógica de negocio
- mantén `OrderApi` como host y orquestador
- usa `OrderUI` para experiencias de cliente independientes
