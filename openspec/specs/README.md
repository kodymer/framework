# CompanyName Enterprise Modular Framework - Technical Specification

**Version**: 1.0.0-rc | **Status**: Draft | **Last Updated**: 2026-05-13

---

## 📖 Overview

Esta especificación técnica documenta el **CompanyName Enterprise Modular Framework**, un marco de trabajo profesional construido con **Domain-Driven Design (DDD)**, **CQRS** y **Clean Architecture** para .NET 8.0.

El framework está diseñado para:
- ✅ Acelerar el desarrollo empresarial mediante plantillas reutilizables (`dotnet new`)
- ✅ Proporcionar módulos transversales pre-configurados (caché, eventos, auditoría, seguridad, etc.)
- ✅ Implementar patrones arquitectónicos probados (DDD, Specification Pattern, Unit of Work)
- ✅ Facilitar la extensión y mantenimiento a través de interfaces bien definidas
- ✅ Soportar múltiples estrategias de acceso a datos (EF Core + Dapper + Specification Pattern)

**Empresa**: Verisure | **Desarrollador**: Capgemini Engineering

---

## 🏗️ Estructura de la Especificación

### 1️⃣ [Visión y Arquitectura](01_vision_arquitectura/README.md)
Fundamentos del framework, principios arquitectónicos, capas y patrones DDD.

- [01 - Visión General](01_vision_arquitectura/01-vision-general.md) — Qué es, para quién, beneficios
- [02 - Principios Arquitectónicos](01_vision_arquitectura/02-principios-arquitectonicos.md) — SOLID, SoC, convenciones
- [03 - Capas de Arquitectura](01_vision_arquitectura/03-capas-arquitectura.md) — Responsabilidades y dependencias
- [04 - Patrones DDD](01_vision_arquitectura/04-patrones-ddd.md) — Agregates, Value Objects, Domain Services

### 2️⃣ [Módulos Transversales](02_modulos_transversales/README.md)
Diez módulos configurables y extensibles que proporcionan funcionalidad compartida.

**Core Infrastructure**:
- [01 - Caching (Hybrid + Redis)](02_modulos_transversales/01-caching.md)
- [02 - EventBus (Local + Azure)](02_modulos_transversales/02-eventbus.md)
- [03 - ServiceBus (Local + Azure)](02_modulos_transversales/03-servicebus.md)

**Data Access & Services**:
- [04 - Repository Pattern (EF Core + Dapper)](02_modulos_transversales/04-repository-pattern.md)
- [05 - Domain & Application Base Services](02_modulos_transversales/05-base-services.md)

**Enterprise Features**:
- [06 - Auditing](02_modulos_transversales/06-auditing.md)
- [07 - Security & Claims](02_modulos_transversales/07-security.md)
- [08 - Localization](02_modulos_transversales/08-localization.md)
- [09 - Logging & Application Insights](02_modulos_transversales/09-logging.md)
- [10 - Validation (FluentValidation)](02_modulos_transversales/10-validation.md)

### 3️⃣ [Plantillas y Generación de Código](03_plantillas/README.md)
Cómo instalar y usar el paquete de templates base y las plantillas concretas.

- [01 - Instalación del Paquete Base](03_plantillas/01-instalacion-paquete-base.md)
- [02 - Plantilla ASP.NET Core Module](03_plantillas/02-template-aspnetcore-module.md)
- [03 - Plantilla ASP.NET Core WebAPI](03_plantillas/03-template-aspnetcore-webapi.md)
- [04 - Plantilla Vue.js Frontend](03_plantillas/04-template-vuejs.md)
- [05 - Estructura Generada y Post-Setup](03_plantillas/05-estructura-generada.md)
- [06 - Workflow Completo](03_plantillas/06-workflow-completo.md)

### 4️⃣ [Guías de Implementación](04_guias_implementacion/README.md)
Guías paso-a-paso para tareas comunes.

- [01 - Crear un Módulo Nuevo](04_guias_implementacion/01-crear-modulo-nuevo.md)
- [02 - Extender el Framework](04_guias_implementacion/02-extender-framework.md)
- [03 - Configurar Módulos Transversales](04_guias_implementacion/03-configurar-transversales.md)
- [04 - Testing Patterns](04_guias_implementacion/04-testing-patterns.md)

### 5️⃣ [Ejemplos Prácticos](05_ejemplos_practicos/README.md)
Código ejecutable y realista.

- [01 - CRUD Simple con DDD](05_ejemplos_practicos/01-ejemplo-crud-simple.md)
- [02 - Domain Events y Event Publishing](05_ejemplos_practicos/02-ejemplo-domain-events.md)
- [03 - Caching + Redis + Invalidation](05_ejemplos_practicos/03-ejemplo-caching-redis.md)
- [04 - Transacciones Distribuidas (Saga Pattern)](05_ejemplos_practicos/04-ejemplo-distributed-transactions.md)

### 6️⃣ [Decisiones Arquitectónicas (ADRs)](06_decisiones_arquitectonicas/README.md)
Justificación de las decisiones clave del framework.

- [ADR-001 - Por qué Domain-Driven Design](06_decisiones_arquitectonicas/adr-001-ddd-pattern.md)
- [ADR-002 - Specification Pattern vs Query Objects](06_decisiones_arquitectonicas/adr-002-specification-pattern.md)
- [ADR-003 - Versioning Único del Framework](06_decisiones_arquitectonicas/adr-003-versioning-unico.md)
- [ADR-004 - Multi-tenancy Strategy](06_decisiones_arquitectonicas/adr-004-multi-tenant.md)
- [ADR-005 - Plugin & Extension Strategy](06_decisiones_arquitectonicas/adr-005-plugin-strategy.md)

---

## 🚀 Cómo Usar Esta Especificación

### Para Arquitectos y Líderes Técnicos
1. Lee [Visión General](01_vision_arquitectura/01-vision-general.md)
2. Revisa [Principios Arquitectónicos](01_vision_arquitectura/02-principios-arquitectonicos.md)
3. Consulta [Decisiones Arquitectónicas (ADRs)](06_decisiones_arquitectonicas/README.md) para entender justificaciones

### Para Desarrolladores Nuevos
1. Comienza con [Guía: Crear un Módulo Nuevo](04_guias_implementacion/01-crear-modulo-nuevo.md)
2. Revisa [Ejemplos Prácticos](05_ejemplos_practicos/README.md)
3. Consulta módulos específicos según necesidad

### Para Integrar Módulos Transversales
1. Lee la documentación del [módulo específico](02_modulos_transversales/README.md)
2. Sigue la sección "Configuración en Program.cs"
3. Consulta ejemplos en [Ejemplos Prácticos](05_ejemplos_practicos/README.md)

### Para Extender el Framework
1. Lee [Guía: Extender el Framework](04_guias_implementacion/02-extender-framework.md)
2. Revisa [Decisiones Arquitectónicas](06_decisiones_arquitectonicas/README.md) para alineación
3. Proporciona patches siguiendo patrones existentes

---

## 📊 Estructura de Cada Módulo Transversal

Cada módulo sigue esta estructura uniforme para máxima claridad:

```
1. Overview
   ├── Propósito y casos de uso
   ├── Stack de tecnología
   └── Cuándo usar este módulo

2. Abstracciones Principales
   ├── Interfaces clave
   ├── Clases abstractas
   └── Namespaces e identidades

3. Comportamiento Default
   ├── Implementación out-of-box
   └── Configuración mínima requerida

4. Configuración en Program.cs
   ├── Extension methods (AddXXX)
   ├── Parámetros y opciones
   └── Ejemplo completo

5. Patrones de Uso
   ├── Inyección de dependencias
   ├── Ejemplos prácticos
   └── Best practices

6. Extensión Personalizada
   ├── Interfaces a implementar
   ├── Registro en DI
   └── Ejemplo step-by-step

7. Testing
   ├── Mock/stub strategies
   ├── Test patterns
   └── Ejemplos de tests
```

---

## 🔧 Información del Framework

| Aspecto | Valor |
|--------|-------|
| **Version** | 1.0.0-rc |
| **Target Framework** | .NET 8.0 |
| **Empresa** | Verisure |
| **Desarrollador** | Capgemini Engineering |
| **Patrón Principal** | Domain-Driven Design + CQRS + Clean Architecture |
| **Versionado** | Único para todo el framework |
| **Estado** | Draft (En revisión) |

---

## 📚 Metadatos

Ver [SPECIFICATION.yaml](SPECIFICATION.yaml) para metadatos completos en formato OpenSpec.

---

## 📝 Changelog

Historial de cambios y versiones en [CHANGELOG.md](CHANGELOG.md)

---

## ✅ Criterios de Calidad

Esta especificación cumple con los siguientes criterios:

- ✅ Auto-contenida (sin dependencias externas no explicadas)
- ✅ Cada módulo tiene 7 secciones completas
- ✅ Mínimo 2 ejemplos de código por módulo
- ✅ Plantillas documentadas con parámetros CLI claros
- ✅ Guías con checklists y ejemplos end-to-end
- ✅ ADRs en formato estándar (Contexto → Alternativas → Decisión → Consecuencias)
- ✅ Versionado consistente (1.0.0-rc)
- ✅ Estructura navegable desde este README

---

## 🤝 Contribuir

Para contribuir a esta especificación:
1. Sigue el formato de cada sección
2. Mantén consistencia con patrones existentes
3. Añade ejemplos prácticos cuando sea posible
4. Actualiza [CHANGELOG.md](CHANGELOG.md)
5. Valida que la especificación siga siendo auto-contenida

---

**Última actualización**: 2026-05-13 | **Versión spec**: 1.0.0-rc
