# ADR-005: Estrategia de extensibilidad con DI y Marker Interfaces

## 1. Título

Extender el framework mediante Dependency Injection y Marker Interfaces, evitando runtime plugin loading.

## 2. Contexto

El framework debe ser extensible sin requerir cambios constantes al core.
Se debía elegir un enfoque para agregar nuevas implementaciones y reemplazar servicios.

## 3. Alternativas consideradas

### 3.1 Plugins dinámicos en runtime
- Ventajas: alta flexibilidad
- Desventajas: complejidad, seguridad y soporte limitado

### 3.2 Extensiones mediante conventions y DI
- Ventajas: simple y consistente
- Desventajas: menor flexibilidad que plugins dinámicos

### 3.3 Módulos NuGet independientes con explicit references
- Ventajas: control y versionado claro
- Desventajas: requiere referencias adicionales manuales

## 4. Decisión

Se adoptó una estrategia basada en DI y Marker Interfaces, con uso de `AddXxx` extensible y registros en `IServiceCollection`.

## 5. Consecuencias

### Beneficios
- Extensión simple y segura
- Mantiene el core ligero
- Facilita reemplazos de implementación en build time

### Costos/Trade-offs
- No se soporta carga de plugins arbitrarios en runtime
- Requiere referencia explícita de paquetes o proyectos

## 6. Implicaciones

- Los nuevos módulos deben exponer métodos `AddXxx` de configuración.
- El core debe proveer marker interfaces para clasificación de servicios.
- La documentación debe enseñar la forma estándar de extender el framework.
