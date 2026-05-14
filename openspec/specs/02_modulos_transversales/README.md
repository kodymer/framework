# 2️⃣ Módulos Transversales

**Esta sección documenta exhaustivamente los 10 módulos configurables y extensibles del framework.**

---

## 📚 Módulos Documentados

### Core Infrastructure

| Módulo | Documentación | Stack Tecnológico |
|--------|---------------|-----------------|
| **Caching** | [01 - Caching (Hybrid + Redis)](01-caching.md) | Microsoft.Extensions.Caching.Hybrid, StackExchange.Redis |
| **EventBus** | [02 - EventBus (Local + Azure)](02-eventbus.md) | Local in-process, Azure.Messaging.ServiceBus |
| **ServiceBus** | [03 - ServiceBus (Local + Azure)](03-servicebus.md) | Local queue simulation, Azure.Messaging.ServiceBus |

### Data Access & Services

| Módulo | Documentación | Stack Tecnológico |
|--------|---------------|-----------------|
| **Repository Pattern** | [04 - Repository Pattern (EF Core + Dapper)](04-repository-pattern.md) | EF Core 9.0.11, Dapper 2.1.66, Ardalis.Specification |
| **Base Services** | [05 - Domain & Application Base Services](05-base-services.md) | Herencia, DI, Inyección de dependencias |

### Enterprise Features

| Módulo | Documentación | Stack Tecnológico |
|--------|---------------|-----------------|
| **Auditing** | [06 - Auditing](06-auditing.md) | EF Core interceptors, Automatic tracking |
| **Security** | [07 - Security & Claims](07-security.md) | System.Security.Claims, Authorization |
| **Localization** | [08 - Localization](08-localization.md) | Microsoft.Extensions.Localization |
| **Logging** | [09 - Logging & Application Insights](09-logging.md) | Microsoft.ApplicationInsights, Structured logging |
| **Validation** | [10 - Validation (FluentValidation)](10-validation.md) | FluentValidation 12.1.1 |

---

## 📖 Estructura Uniforme de Cada Módulo

Todos los módulos siguen esta estructura para máxima consistencia:

```
1. Overview
   ├── Propósito
   ├── Casos de uso
   └── Stack de tecnología

2. Abstracciones Principales
   ├── Interfaces clave
   ├── Clases abstractas
   └── Namespaces

3. Comportamiento Default
   ├── Implementación out-of-box
   └── Configuración mínima

4. Configuración en Program.cs
   ├── Extension methods (AddXXX)
   ├── Parámetros opcionales
   └── Ejemplo completo

5. Patrones de Uso
   ├── Inyección de dependencias
   ├── Ejemplos prácticos
   └── Best practices

6. Extensión Personalizada
   ├── Interfaces a implementar
   ├── Cómo registrar custom implementations
   └── Ejemplo step-by-step

7. Testing
   ├── Mock/stub recommendations
   ├── Test patterns
   └── Ejemplos de tests
```

---

## 🎯 Cómo Usar Esta Sección

### Por Tipo de Necesidad

#### "Necesito configurar un módulo específico"
1. Busca el módulo en la tabla anterior
2. Abre su documentación
3. Ve a sección "4. Configuración en Program.cs"
4. Copia el ejemplo y ajusta tu `Program.cs`

#### "Quiero extender un módulo"
1. Lee la documentación del módulo
2. Ve a sección "6. Extensión Personalizada"
3. Implementa la interfaz requerida
4. Registra en DI (Autofac)

#### "Necesito escribir tests para un módulo"
1. Consulta la documentación del módulo
2. Ve a sección "7. Testing"
3. Usa los patterns y mocks sugeridos
4. Copia los ejemplos de tests

#### "Quiero aprender sobre un patrón específico"
1. Lee la sección "Overview" del módulo
2. Revisa "Patrones de Uso" con ejemplos
3. Consulta los ejemplos prácticos en [05_ejemplos_practicos/](../05_ejemplos_practicos/README.md)

---

## 📊 Matriz de Decisión

**¿Cuál módulo necesito?**

| Problema | Módulo | Razón |
|----------|--------|-------|
| Datos se cargan lentamente | Caching | Reduce acceso a BD con Redis o in-memory hybrid cache |
| Servicios necesitan comunicarse | EventBus | Desacoplamiento mediante domain/integration events |
| Necesito colas de mensajes | ServiceBus | Async messaging con Azure Service Bus o local queue |
| Tengo múltiples fuentes de datos | Repository Pattern | Abstracción de acceso a datos con EF Core o Dapper |
| Servicios son largos y repetitivos | Base Services | Clases base aceleran desarrollo |
| Necesito saber quién cambió qué | Auditing | Tracking automático de cambios |
| Necesito roles/permisos | Security | Claims-based authorization |
| App global con múltiples idiomas | Localization | Resource-based translations |
| Necesito observabilidad | Logging | Structured logging + Application Insights |
| Validar input del usuario | Validation | FluentValidation rules |

---

## ✨ Característica Destacada: Base Services

Una característica especial del framework es el uso de **clases base de servicios** que aceleran desarrollo:

- **Domain Service Base** — Funcionalidad común para servicios de dominio
- **Application Service Base** — Funcionalidad común para servicios de aplicación
- Herencia permite reutilizar validaciones, logging, auditoría automáticamente

Ver [05 - Domain & Application Base Services](05-base-services.md) para detalles.

---

## 🔗 Links Relacionados

- [Visión y Arquitectura](../01_vision_arquitectura/README.md) — Conceptos fundamentales
- [Ejemplos Prácticos](../05_ejemplos_practicos/README.md) — Código real con módulos
- [Guías de Implementación](../04_guias_implementacion/README.md) — Cómo integrar módulos

---

**Estado**: ✅ Completed (Fase 3)  
**Documentos**: 10 (10 generados)
