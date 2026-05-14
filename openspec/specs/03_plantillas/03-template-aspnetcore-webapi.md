# 03 - Template ASP.NET Core WebAPI

**Sección**: Plantillas y Generación de Código | **Documento**: 03 de 06 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Crear un proyecto Web API completo con Host, listo para ejecutarse y exponer endpoints HTTP.

### Casos de uso
- microservicios independientes
- APIs de backend con swagger y configuración de lote
- servicios que necesitan su propio ciclo de vida y despliegue

### Stack tecnológico
- `ASP.NET Core` Web API
- `EF Core`
- `Dapper`
- `AutoMapper`
- `ApplicationInsights`

---

## 2. Template Metadata

- `name`: `ASP.NET Core Web API`
- `identity`: `CompanyName.AspNetCore.WebApi`
- `shortName`: `CompanyNameLower-webapi`
- `sourceName`: `ProjectName`

---

## 3. Lo que genera

Estructura principal:

```
ProjectName.sln
src/
  ProjectName.Application/
  ProjectName.Dapper/
  ProjectName.Domain/
  ProjectName.Domain.Shared/
  ProjectName.EntityFrameworkCore/
  ProjectName.HttpApi/
  ProjectName.HttpApi.Host/

test/
  ProjectName.Application.Tests/
  ProjectName.Dapper.Tests/
  ProjectName.Domain.Tests/
  ProjectName.EntityFrameworkCore.Tests/
  ProjectName.HttpApi.Tests/
```

### Notas
- Incluye `HttpApi.Host` como aplicación ejecutable.
- Prepara la solución para despliegues en contenedor y entornos cloud.

---

## 4. Comando de generación

```bash
dotnet new CompanyNameLower-webapi -n OrderApi
```

### Parámetros clave
- `-n`, `--name`: Nombre del servicio API
- `--company`: Nombre de la compañía

---

## 5. Patrones de Uso

### Inicializar la API
- `ProjectName.HttpApi.Host` contiene el entrypoint con `Program.cs`
- `ProjectName.HttpApi` define contratos, endpoints y DTOs
- `ProjectName.Application` orquesta la lógica de negocio

### Mejor práctica
- Usa `ProjectName.EntityFrameworkCore` para persistencia
- Usa `CompanyName.EventBus` o `CompanyName.ServiceBus` para integración asincrónica
- Incluye middleware de logging y auditoría desde el host

---

## 6. Agregar módulos

### Ejemplo
1. Genera `OrderModule` con el template module
2. Añade referencia a `OrderModule.Domain` y `OrderModule.Application` en `OrderApi.HttpApi.Host`
3. Configura DI para los servicios del módulo

---

## 7. Verificación

### Build
```bash
dotnet build OrderApi.sln
```

### Run
```bash
cd OrderApi/src/OrderApi.HttpApi.Host
dotnet run
```
