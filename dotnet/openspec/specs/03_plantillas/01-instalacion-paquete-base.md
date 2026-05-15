# 01 - Instalación del Paquete Base

**Sección**: Plantillas y Generación de Código | **Documento**: 01 de 06 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Explicar cómo instalar el paquete de templates de CompanyName en el CLI de .NET para que los templates estén disponibles con `dotnet new`.

### Casos de uso
- preparar un entorno local de desarrollo
- registrar los templates desde el código fuente
- instalar un paquete local o de NuGet para múltiples desarrolladores

---

## 2. Requisitos Previos

- .NET SDK 8.0 instalado
- `dotnet` en el PATH
- acceso a la carpeta del paquete de templates

---

## 3. Instalación Local

### Comando
```bash
dotnet new -i d:\accelerator\framework\templates\src\CompanyName.Templates
```

### Resultado
- Registro del paquete `CompanyName.Templates`
- Disponibilidad de los siguientes templates:
  - `CompanyNameLower-module`
  - `CompanyNameLower-webapi`
  - `CompanyNameLower-vue`

---

## 4. Verificar Instalación

### Comando
```bash
dotnet new list CompanyName
```

### Salida esperada
Deberías ver los tres templates listados con su `shortName`.

---

## 5. Desinstalación

### Comando
```bash
dotnet new -u d:\accelerator\framework\templates\src\CompanyName.Templates
```

---

## 6. Notas

- Si el paquete se publica como NuGet, reemplaza el path por el nombre del paquete.
- Si se instala desde un path relativo, úsalos con la ruta correcta desde tu carpeta actual.

---

## 7. Troubleshooting

- Si `dotnet new list` no muestra el template, limpia el cache local:
```bash
dotnet new --debug:reinit
```
- Si el paquete está instalado pero con otro nombre, revisa el `shortName` en `.template.config/template.json`.
