# ADR-001: Uso de Domain-Driven Design (DDD)

## 1. Título

Uso de Domain-Driven Design (DDD) como patrón arquitectónico principal.

## 2. Contexto

El framework se construye para aplicaciones empresariales complejas que requieren:
- modelar reglas de negocio ricas
- mantener consistencia en dominios cambiantes
- facilitar la colaboración entre expertos de negocio y desarrolladores

Se necesitaba un enfoque que permitiera:
- separar lógica de negocio de infraestructura
- evitar modelos anémicos
- gestionar agregados y invariantes de dominio

## 3. Alternativas consideradas

### 3.1 CRUD convencional
- Ventajas: rápido de implementar, simple
- Desventajas: lógica de negocio mezclada con persistencia, difícil mantenimiento en dominios complejos

### 3.2 Arquitectura basada en servicios transaccionales
- Ventajas: adecuada para operaciones simples
- Desventajas: no maneja bien invariantes complejas ni evolución de reglas de negocio

### 3.3 Microservicios sin DDD
- Ventajas: alta modularidad
- Desventajas: riesgo de diseño anémico, duplicación de lógica, dificultad para definir boundaries claros

## 4. Decisión

Se decidió adoptar Domain-Driven Design como la guía principal para los componentes de dominio y la estructura de capas.

## 5. Consecuencias

### Beneficios
- El dominio contiene la lógica de negocio real
- Agregados y Value Objects encapsulan invariantes
- Las capas de aplicación y presentación se mantienen ligeras
- Se facilita la comunicación entre expertos de negocio y desarrolladores

### Costos/Trade-offs
- Mayor complejidad inicial
- Curva de aprendizaje para desarrolladores nuevos
- Requiere disciplina en naming y boundaries

## 6. Implicaciones

- Todos los nuevos módulos deben usar Aggregates, Value Objects y Domain Services.
- El framework debe incluir base classes para ApplicationService y AggregateRoot.
- La documentación debe enseñar patrones DDD claramente.
