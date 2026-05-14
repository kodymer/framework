# ADR-002: Uso del Specification Pattern

## 1. Título

Adopción del Specification Pattern para consultas reutilizables y type-safe.

## 2. Contexto

En el framework se requieren consultas complejas sobre agregados y repositorios.
Las consultas deben ser reutilizables, composables y desacopladas de la infraestructura.

## 3. Alternativas consideradas

### 3.1 Query Objects
- Ventajas: encapsula consultas como objetos
- Desventajas: menos composabilidad y puede duplicar lógicas de filtrado

### 3.2 Strings SQL directos
- Ventajas: flexibilidad máxima
- Desventajas: acoplamiento a implementación, riesgo de inyección, difícil mantenimiento

### 3.3 Expression Builders personalizados
- Ventajas: control completo sobre las expresiones
- Desventajas: mayor complejidad y código boilerplate

## 4. Decisión

Se adoptó el Specification Pattern para las consultas en los repositorios, usando `ISpecification<T>` y un evaluador que traduce a EF Core o Dapper según sea necesario.

## 5. Consecuencias

### Beneficios
- Reutilización de consultas comunes
- Composición de filtros y ordenamientos
- Separación entre definición de consulta y ejecución

### Costos/Trade-offs
- Necesidad de una capa adicional de abstracción
- Curva de aprendizaje para escribir especificaciones
- Posible sobreingeniería en consultas muy simples

## 6. Implicaciones

- Los repositorios deben exponer métodos que acepten `ISpecification<T>`.
- Las implementaciones EF Core y Dapper deben soportar el patrón.
- La documentación y ejemplos deben incluir especificaciones comunes.
