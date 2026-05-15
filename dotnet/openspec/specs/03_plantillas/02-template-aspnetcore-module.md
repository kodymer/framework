# 02 - Template ASP.NET Core Module

**Sección**: Plantillas y Generación de Código | **Documento**: 02 de 06 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Crear un módulo ASP.NET Core reutilizable compatible con el framework modular del proyecto.

### Casos de uso
- servicios de negocio compartidos entre APIs
- módulos de dominio que se instancian en varias soluciones
- desarrollo de funcionalidades independientes con su propia capa de aplicación

### Stack tecnológico
- `dotnet new` templates
- `ASP.NET Core`
- `EF Core` + `Dapper`
- `AutoMapper`

---

## 2. Template Metadata

- `name`: `ASP.NET Core Module`
- `identity`: `CompanyName.AspNetCore.Module`
- `shortName`: `CompanyNameLower-module`
- `sourceName`: `ProjectName`

---

## 3. Lo que genera

El template crea una solución con la siguiente estructura principal:

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
- Aunque incluye `HttpApi.Host`, el enfoque es construir un módulo reutilizable que puede integrarse en soluciones más grandes.
- El template preconfigura el enlace entre dominio, aplicación y persistencia.

---

## 4. Comando de generación

```bash
dotnet new CompanyNameLower-module -n OrderModule
```

### Parámetros clave
- `-n`, `--name`: Nombre del módulo
- `--company`: Nombre de la compañía (usado en namespaces y branding si está habilitado)

---

## 5. Patrones de Uso

### Reutilizar el módulo
- Importa `OrderModule.Domain` y `OrderModule.Application` en otro proyecto
- Usa `OrderModule.EntityFrameworkCore` para persistencia
- Consume `OrderModule.HttpApi` o `OrderModule.HttpApi.Host` según necesidad

### Mejor práctica
- Mantén la lógica de negocio en `ProjectName.Domain`
- Usa `ProjectName.Application` para orquestar casos de uso
- Usa `ProjectName.Dapper` para consultas de alto rendimiento

---

## 6. Personalización post-generación

### Cambiar el nombre del proyecto
- Reemplaza `ProjectName` por el nombre real en archivos de solución
- Asegúrate de actualizar namespaces

### Agregar otro módulo
- Crea múltiples módulos con el mismo paquete base
- Combina módulos desde un `WebApi` o solución principal

---

## 7. Verificación

### Build
```bash
dotnet build OrderModule.sln
```

### Ejecutar tests
```bash
dotnet test OrderModule.sln
```
