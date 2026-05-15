# 6️⃣ Decisiones Arquitectónicas (ADRs)

**Esta sección documenta el "por qué" detrás de las decisiones clave del framework.**

---

## 📚 Documentos

| Documento | Pregunta | Decisión |
|-----------|----------|----------|
| [ADR-001](adr-001-ddd-pattern.md) | ¿Por qué DDD? | Usar Domain-Driven Design para problemas complejos |
| [ADR-002](adr-002-specification-pattern.md) | ¿Specification vs Query Objects? | Specification Pattern para flexibilidad |
| [ADR-003](adr-003-versioning-unico.md) | ¿Versioning independiente o único? | Versión única del framework (1.0.0-rc) |
| [ADR-004](adr-004-multi-tenant.md) | ¿Multi-tenancy? | Estrategia: Row-level security + Partitioned data |
| [ADR-005](adr-005-plugin-strategy.md) | ¿Cómo extender el framework? | Plugin vía DI + Marker Interfaces (no runtime loading) |

---

## 🎯 ¿Qué es un ADR?

Un **Architecture Decision Record (ADR)** documenta una decisión arquitectónica importante de manera estructurada:

```
1. TÍTULO
   ¿Qué decisión se toma?

2. CONTEXTO
   ¿Cuál es el problema o la situación?
   ¿Qué restricciones existen?

3. ALTERNATIVAS CONSIDERADAS
   ¿Qué otras opciones había?
   ¿Por qué no se eligieron?

4. DECISIÓN
   ¿Qué se decidió?
   ¿Por qué esta opción?

5. CONSECUENCIAS
   ¿Cuáles son los beneficios?
   ¿Cuáles son los costos/trade-offs?

6. IMPLICACIONES
   ¿Cómo afecta a futuros desarrollos?
   ¿Qué dependencias crea?
```

---

## 📖 Lectura Recomendada

### Si eres Arquitecto:
Lee todos los ADRs para entender la estrategia completa del framework.

### Si eres Desarrollador Senior:
Lee los ADRs relevantes a tu módulo/feature.

### Si eres Desarrollador Nuevo:
Lee [ADR-001 - DDD Pattern](adr-001-ddd-pattern.md) primero para entender la filosofía.

---

## 🔄 Cómo Usar ADRs

### Entender Decisiones Pasadas
- ¿Por qué se eligió Specification Pattern?
  → Lee [ADR-002](adr-002-specification-pattern.md)

- ¿Por qué versión única del framework?
  → Lee [ADR-003](adr-003-versioning-unico.md)

### Justificar Nuevas Decisiones
Cuando debas tomar una decisión arquitectónica:
1. Revisa ADRs existentes para consistency
2. Crea un nuevo ADR siguiendo el formato
3. Documenta contexto, alternativas, decisión
4. Comparte con el equipo

### Reabrir Decisiones
Si una decisión ya no es válida:
1. Crea un ADR que la superse (e.g., ADR-006: "Supersedes ADR-003")
2. Documenta qué cambió y por qué
3. Actualiza la implementación
4. Mantén los ADRs anteriores para historial

---

## 🗺️ Matriz de ADRs vs Decisiones del Framework

| Aspecto | ADR | Status |
|--------|-----|--------|
| **Patrón de Diseño** | ADR-001 (DDD) | ✅ Activo |
| **Data Query Pattern** | ADR-002 (Specification) | ✅ Activo |
| **Versionado** | ADR-003 (Único) | ✅ Activo |
| **Multi-tenancy** | ADR-004 (Row-level) | 🔄 En revisión |
| **Extensibilidad** | ADR-005 (DI Plugins) | ✅ Activo |

---

## 🔗 Links Relacionados

- [Visión y Arquitectura](../01_vision_arquitectura/README.md) — Conceptos fundamentales
- [Módulos Transversales](../02_modulos_transversales/README.md) — Implementación de decisiones
- [Ejemplos Prácticos](../05_ejemplos_practicos/README.md) — Código que refleja ADRs

---

## 📚 Referencia Adicional

**Para más información sobre ADRs**:
- [ADR GitHub template](https://github.com/joelparkerhenderson/architecture_decision_record)
- Libro: "Building Evolutionary Architectures" (Ford, Parsons, Kua)

---

**Estado**: ✅ Completed (Fase 7)  
**Documentos**: 5 generados  
**Enfoque**: Justificación y historial de decisiones clave
