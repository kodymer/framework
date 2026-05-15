# 05 - Estructura Generada y Post-Setup

**Sección**: Plantillas y Generación de Código | **Documento**: 05 de 06 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Describir la estructura de archivos que generan los templates y qué pasos realizar inmediatamente después de crear el proyecto.

### Casos de uso
- entender qué se generó
- verificar la solución
- completar la configuración inicial

---

## 2. Estructura Generada

### ASP.NET Core Module

```
OrderModule.sln
src/
  OrderModule.Application/
  OrderModule.Dapper/
  OrderModule.Domain/
  OrderModule.Domain.Shared/
  OrderModule.EntityFrameworkCore/
  OrderModule.HttpApi/
  OrderModule.HttpApi.Host/
test/
  OrderModule.Application.Tests/
  OrderModule.Dapper.Tests/
  OrderModule.Domain.Tests/
  OrderModule.EntityFrameworkCore.Tests/
  OrderModule.HttpApi.Tests/
```

### ASP.NET Core WebAPI

```
OrderApi.sln
src/
  OrderApi.Application/
  OrderApi.Dapper/
  OrderApi.Domain/
  OrderApi.Domain.Shared/
  OrderApi.EntityFrameworkCore/
  OrderApi.HttpApi/
  OrderApi.HttpApi.Host/
test/
  OrderApi.Application.Tests/
  OrderApi.Dapper.Tests/
  OrderApi.Domain.Tests/
  OrderApi.EntityFrameworkCore.Tests/
  OrderApi.HttpApi.Tests/
```

### Vue.js Frontend

```
OrderUI/
  package.json
  vite.config.ts
  tsconfig.json
  Dockerfile
  public/
  src/
    components/
    views/
    services/
    stores/
  cypress/
  Deployment/
  .vscode/
```

---

## 3. Post-Setup Inmediato

### 1. Abrir la solución
- Verifica que el `.sln` haya sido generado correctamente
- Revisa los proyectos y referencias

### 2. Restaurar dependencias
```bash
dotnet restore OrderModule.sln
```

### 3. Build y tests básicos
```bash
dotnet build OrderModule.sln
dotnet test OrderModule.sln
```

### 4. Configurar variables de entorno
- para `WebAPI`: `DefaultConnection`, `ApplicationInsights:ConnectionString`, `AzureServiceBus`
- para `Vue.js`: URL base del API en `env.template`

---

## 4. Ajustes comunes después de generar

### Project name y namespaces
- Revisa `ProjectName` y `ProjectNameLower`
- Actualiza namespaces si necesitas un nombre distinto al original

### Ajustar `Program.cs`
- Agrega módulos transversales: `AddCaching`, `AddEventBusLocal`, `AddValidation`
- Configura seguridad, logging y localization

### Integrar módulos compartidos
- Si generaste un módulo independiente, referencia sus proyectos desde tu WebAPI
- Confirma que el DI registre los servicios de cada módulo

---

## 5. Verificación de la generación

### Generar y revisar
```bash
dotnet new CompanyNameLower-module -n OrderModule
dotnet new CompanyNameLower-webapi -n OrderApi
dotnet new CompanyNameLower-vue -n OrderUI
```

### Revisión rápida
- `OrderModule.sln` existe
- `OrderApi.sln` existe
- `OrderUI/package.json` existe

---

## 6. Notas

- El template Vue no incluye servidor .NET, es un proyecto frontend independiente.
- El template WebAPI puede actuar como host principal para módulos de negocio.
