# ADR-004: Enfoque de multi-tenancy

## 1. Título

Adopción de multi-tenancy con seguridad a nivel de filas y datos particionados.

## 2. Contexto

El framework debe soportar aplicaciones que atienden a múltiples clientes o tenants.
Es necesario definir un enfoque que equilibrio seguridad, escalabilidad y complejidad.

## 3. Alternativas consideradas

### 3.1 Base de datos por tenant
- Ventajas: aislamiento máximo
- Desventajas: costos operativos altos, complejidad de gestión

### 3.2 Schema por tenant
- Ventajas: aislamiento medio
- Desventajas: aumenta la complejidad de consultas y migraciones

### 3.3 Row-level security / particionado en una misma base
- Ventajas: menor complejidad operativa, buen equilibrio entre aislamiento y rendimiento
- Desventajas: mayor responsabilidad en la capa de datos y aplicación

## 4. Decisión

Se adoptó un enfoque basado en row-level security y particionado lógico de datos, con soporte para tenant identification en la capa de aplicación.

## 5. Consecuencias

### Beneficios
- Escalabilidad con menor overhead operativo
- Menor complejidad de despliegue que múltiples bases
- Políticas de seguridad centralizadas en la capa de datos

### Costos/Trade-offs
- Necesidad de validación de tenant en todas las consultas
- Posible impacto de rendimiento para tablas muy grandes
- Requiere diseño cuidadoso de índices y particiones

## 6. Implicaciones

- Repositorios y especificaciones deben incluir filtros por tenant.
- `ICurrentUser` debe exponer `TenantId`.
- La documentación debe definir cómo habilitar multi-tenancy en cada módulo.
