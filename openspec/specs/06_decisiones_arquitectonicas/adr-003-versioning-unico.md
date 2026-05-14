# ADR-003: Versión única del framework

## 1. Título

Uso de un único esquema de versionado para todo el framework.

## 2. Contexto

El framework incluye múltiples módulos y plantillas conectadas.
Era necesario decidir entre versionar cada módulo por separado o mantener una versión unificada del framework.

## 3. Alternativas consideradas

### 3.1 Versionado por módulo
- Ventajas: mayor granularidad
- Desventajas: mayor complejidad de compatibilidad y coordinación

### 3.2 Versionado por plantilla y módulo
- Ventajas: independencia de despliegue
- Desventajas: alto costo de mantenimiento y pruebas cruzadas

## 4. Decisión

Se decidió usar una versión única para todo el framework: `1.0.0-rc` en la fase actual.

## 5. Consecuencias

### Beneficios
- Simplifica el versionado y el packaging
- Garantiza consistencia de dependencias entre módulos
- Facilita la comunicación de release al equipo

### Costos/Trade-offs
- Menor flexibilidad para cambios independientes de un módulo
- Requiere coordinación de releases entre equipos

## 6. Implicaciones

- El `SPECIFICATION.yaml` y el changelog deben reflejar la versión global.
- Todas las documentaciones de módulos y plantillas deben alinearse con la versión.
- Las ramas de desarrollo deben sincronizarse para el release general.
