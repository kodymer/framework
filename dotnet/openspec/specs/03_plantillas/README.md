# 3️⃣ Plantillas y Generación de Código

**Esta sección documenta cómo instalar y usar las plantillas `dotnet new` del framework.**

---

## 📚 Documentos

| Documento | Propósito |
|-----------|----------|
| [01 - Instalación del Paquete Base](01-instalacion-paquete-base.md) | Cómo instalar CompanyName.Templates en dotnet new |
| [02 - Template ASP.NET Core Module](02-template-aspnetcore-module.md) | Módulo sin Host para compartir en el framework |
| [03 - Template ASP.NET Core WebAPI](03-template-aspnetcore-webapi.md) | API HTTP con proyecto Host |
| [04 - Template Vue.js Frontend](04-template-vuejs.md) | Frontend moderno con Vue.js 3 |
| [05 - Estructura Generada y Post-Setup](05-estructura-generada.md) | Qué proyectos se crean y cómo configurarlos |
| [06 - Workflow Completo](06-workflow-completo.md) | Flujo end-to-end: instalar → generar → configurar |

---

## 🚀 Quick Start

### Paso 1: Instalar paquete de templates
```bash
dotnet new -i d:\accelerator\framework\templates\src\CompanyName.Templates
```

### Paso 2: Generar un módulo
```bash
dotnet new CompanyNameLower-module -n OrderModule
```

### Paso 3: Generar una API
```bash
dotnet new CompanyNameLower-webapi -n OrderApi
```

### Paso 4: Generar frontend
```bash
dotnet new CompanyNameLower-vue -n OrderUI
```

---

## 📋 Templates Disponibles

### Paquete Base
**`CompanyName.Templates`**
- Propósito: Registrar los templates en `dotnet new`
- Instalación: `dotnet new -i <path>`
- Resultado: 3 templates disponibles para generar código

### Plantilla 1: ASP.NET Core Module
**shortName**: `CompanyNameLower-module`

```bash
dotnet new CompanyNameLower-module -n MyModule
```

- ✅ Módulo sin Host (para compartir dentro del framework)
- ✅ Contiene: Domain, Application, EF Core, Dapper, Api, Tests
- ✅ Ideal para: Módulos de negocio reutilizables

### Plantilla 2: ASP.NET Core WebAPI
**shortName**: `CompanyNameLower-webapi`

```bash
dotnet new CompanyNameLower-webapi -n MyApi --company Verisure
```

- ✅ API HTTP completa con Host
- ✅ Contiene: Todas las capas + Program.cs + Host configuration
- ✅ Ideal para: APIs independientes, microservicios

### Plantilla 3: Vue.js Frontend
**shortName**: `CompanyNameLower-vue`

```bash
dotnet new CompanyNameLower-vue -n MyUI
```

- ✅ Frontend moderno con Vue.js 3
- ✅ Incluye: TypeScript, Vite, Cypress, Linting
- ✅ Ideal para: Aplicaciones front-end modernas

---

## 📊 Matriz de Decisión: ¿Qué Template Usar?

| Caso de Uso | Template | Razón |
|-----------|----------|-------|
| Lógica de negocio reutilizable | **Module** | Se comparte en múltiples APIs |
| Servicio independiente / Microservicio | **WebAPI** | Ejecutable con su propio Host |
| Dashboard / Admin UI | **Vue.js** | Frontend interactivo moderno |
| Mezcla de Backend + Frontend | **WebAPI** + **Vue.js** | Generar ambos, integrar |

---

## 🔄 Workflow Recomendado

```
1. Instalar paquete base
   ↓
2. Generar módulo de negocio (Module template)
   ↓
3. Generar API Host (WebAPI template)
   ↓
4. Generar UI Front-end (Vue.js template)
   ↓
5. Configurar módulos transversales
   ↓
6. Iniciar desarrollo
```

Ver [06 - Workflow Completo](06-workflow-completo.md) para detalles.

---

## 🎯 Parámetros Principales

### Module Template
```
--name (-n) ............ Nombre del módulo (ej: "OrderModule")
--company .............. Nombre compañía (ej: "Verisure")
```

### WebAPI Template
```
--name (-n) ............ Nombre de la API (ej: "OrderApi")
--company .............. Nombre compañía (ej: "Verisure")
```

### Vue.js Template
```
--name (-n) ............ Nombre del proyecto UI (ej: "OrderUI")
```

---

## 📂 Estructura Generada

### Module Template genera:
```
MyModule.sln
├── src/
│   ├── MyModule.Domain/
│   ├── MyModule.Application/
│   ├── MyModule.EntityFrameworkCore/
│   ├── MyModule.Dapper/
│   ├── MyModule.Api/
│   └── (no Host)
└── test/
    └── MyModule.Tests/
```

### WebAPI Template genera:
```
MyApi.sln
├── src/
│   ├── MyApi.Domain/
│   ├── MyApi.Application/
│   ├── MyApi.EntityFrameworkCore/
│   ├── MyApi.Dapper/
│   ├── MyApi.HttpApi/
│   ├── MyApi.HttpApi.Host/  ← INCLUIDO
│   └── Dockerfile, etc.
└── test/
    └── MyApi.Tests/
```

### Vue.js Template genera:
```
MyUI/
├── src/
│   ├── components/
│   ├── views/
│   ├── services/
│   └── stores/
├── cypress/
├── Dockerfile
├── package.json
├── vite.config.ts
└── deployment configs
```

---

## 🔗 Links Relacionados

- [Guías de Implementación](../04_guias_implementacion/README.md) — Cómo comenzar un proyecto
- [Módulos Transversales](../02_modulos_transversales/README.md) — Configuración post-generación
- [Ejemplos Prácticos](../05_ejemplos_practicos/README.md) — Código real

---

**Estado**: ✅ Completed (Fase 4)  
**Documentos**: 6 generados
