# 04 - Template Vue.js Frontend

**Sección**: Plantillas y Generación de Código | **Documento**: 04 de 06 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Crear un frontend moderno basado en Vue.js 3, listo para desarrollo local y despliegue.

### Casos de uso
- dashboards y aplicaciones web ricas
- paneles administrativos
- frontends para APIs del framework

### Stack tecnológico
- `Vue.js 3`
- `Vite`
- `TypeScript`
- `Cypress`

---

## 2. Template Metadata

- `name`: `CompanyName Vue`
- `identity`: `CompanyName.Vue.App`
- `shortName`: `CompanyNameLower-vue`

---

## 3. Lo que genera

Estructura principal:

```
MyUI/
  package.json
  vite.config.ts
  tsconfig.json
  Dockerfile
  .gitignore
  public/
  src/
    components/
    views/
    services/
    stores/
  cypress/
  Deployment/
  .vscode/
```

### Notas
- Incluye una plantilla de `env.template`.
- Contiene configuraciones para linters y pruebas E2E.

---

## 4. Comando de generación

```bash
dotnet new CompanyNameLower-vue -n OrderUI
```

### Parámetros clave
- `-n`, `--name`: Nombre de la aplicación UI

---

## 5. Patrones de Uso

### Desarrollo local
```bash
cd OrderUI
npm install
npm run dev
```

### Mejor práctica
- Mantén la lógica de UI separada en `services`
- Usa `stores` para estado compartido
- Consume APIs desde `src/services` en lugar de mezclarlas con componentes

---

## 6. Personalización

### Agregar rutas
- Crea nuevas vistas en `src/views`
- Declara rutas en el router local

### Conectar con el backend
- Configura el endpoint API en `src/services/api.ts`
- Usa variables de entorno en `env.template`

---

## 7. Verificación

### Run
```bash
npm install
npm run dev
```

### Tests
```bash
npm run test:unit
npm run test:e2e
```
