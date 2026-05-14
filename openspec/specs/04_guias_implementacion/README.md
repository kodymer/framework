# 4️⃣ Guías de Implementación

**Esta sección proporciona guías paso-a-paso para tareas comunes.**

---

## 📚 Documentos

| Documento | Propósito |
|-----------|----------|
| [01 - Crear un Módulo Nuevo](01-crear-modulo-nuevo.md) | Cómo empezar un módulo desde cero |
| [02 - Extender el Framework](02-extender-framework.md) | Cómo crear nuevos módulos transversales |
| [03 - Configurar Módulos Transversales](03-configurar-transversales.md) | Cómo activar/configurar cada módulo |
| [04 - Testing Patterns](04-testing-patterns.md) | Cómo escribir tests siguiendo patrones del framework |

---

## 🎯 Flujo por Tipo de Tarea

### "Estoy empezando un proyecto nuevo"
→ Lee [01 - Crear un Módulo Nuevo](01-crear-modulo-nuevo.md)

**Incluye**:
- Pre-requisitos
- Paso-a-paso con `dotnet new`
- Estructura generada
- Configuración inicial
- Primer CRUD de ejemplo
- Checklist de completitud

### "Necesito un módulo transversal que no existe"
→ Lee [02 - Extender el Framework](02-extender-framework.md)

**Incluye**:
- Cuándo crear un nuevo módulo
- Pasos: abstracción → implementación → registro
- Patrón de extensión
- Testing del módulo
- Cómo documentarlo

### "Necesito activar Redis, Azure EventBus, etc."
→ Lee [03 - Configurar Módulos Transversales](03-configurar-transversales.md)

**Incluye**:
- Configuración de cada uno de los 10 módulos
- Parámetros obligatorios vs opcionales
- Cómo activar/desactivar
- Ejemplos de Program.cs
- Troubleshooting común

### "Necesito escribir tests de calidad"
→ Lee [04 - Testing Patterns](04-testing-patterns.md)

**Incluye**:
- Unit tests (Domain Services, Application Services)
- Integration tests (Repositories, EF Core)
- End-to-end tests (API endpoints)
- Mock strategies por módulo
- Test data builders

---

## 📋 Checklist General

Después de completar estas guías, deberías poder:

- ✅ Generar un módulo nuevo con `dotnet new`
- ✅ Entender la estructura generada y propósito de cada carpeta
- ✅ Configurar los 10 módulos transversales en tu proyecto
- ✅ Crear un Domain Service, Application Service y Endpoint
- ✅ Escribir tests unitarios e integración
- ✅ Extender el framework con nuevo módulo transversal
- ✅ Deployar a Azure u otro cloud provider
- ✅ Troubleshoot problemas comunes

---

## 🚀 Workflow Recomendado

```
1. LEER: Visión y Arquitectura
   ↓
2. SEGUIR: Crear un Módulo Nuevo (01)
   ↓
3. IMPLEMENTAR: Primer CRUD de ejemplo
   ↓
4. CONFIGURAR: Módulos Transversales (03)
   ↓
5. ESCRIBIR: Tests (04)
   ↓
6. LEER: Ejemplos Prácticos para inspiración
   ↓
7. (Si necesitas extender) SEGUIR: Extender el Framework (02)
```

---

## 🔗 Links Relacionados

- [Plantillas](../03_plantillas/README.md) — Instalación de templates
- [Módulos Transversales](../02_modulos_transversales/README.md) — Referencia de configuración
- [Ejemplos Prácticos](../05_ejemplos_practicos/README.md) — Código real

---

**Estado**: ✅ Completed (Fase 5)  
**Documentos**: 4 generados
