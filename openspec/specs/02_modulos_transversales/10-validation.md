# 10 - Validation (FluentValidation)

**Sección**: Módulos Transversales | **Documento**: 10 de 10 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Proveer validación declarativa de comandos, DTOs y modelos con reglas legibles y reutilizables.

### Casos de uso
- validar request payloads
- validar comandos de aplicación
- validar objetos de dominio antes de persistirlos

### Stack tecnológico
- `FluentValidation`
- `CompanyName.Validation`

---

## 2. Abstracciones Principales

### Interfaces clave
- `IValidator<T>`
- `IValidationResult`

### Clases base
- `AbstractValidator<T>`
- `ValidationResultMapper`

### Namespaces
- `CompanyName.Validation`

---

## 3. Comportamiento Default

### Implementación out-of-box
- validadores para comandos y DTOs
- integración con pipelines de mediador o middleware HTTP
- mapeo de errores en respuestas estándar

### Reglas predeterminadas
- las reglas se definen con expresiones legibles
- los errores retornan listas de mensajes con contexto
- la validación se aplica antes de la lógica de negocio

---

## 4. Configuración en Program.cs

```csharp
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();

builder.Services.AddTransient<IValidator<CreateOrderCommand>, CreateOrderValidator>();
```

---

## 5. Patrones de Uso

### Inyección de dependencias
```csharp
public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.OrderNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Items)
            .NotEmpty()
            .Must(items => items.Count <= 100)
            .WithMessage("Maximum 100 items allowed");
    }
}
```

### Uso práctico
```csharp
var validator = new CreateOrderValidator();
var result = validator.Validate(command);
if (!result.IsValid)
{
    return Results.BadRequest(result.Errors);
}
```

### Best practices
- centraliza validadores por comando/DTO
- evita lógica compleja en reglas
- usa `CascadeMode.Stop` cuando convenga

---

## 6. Extensión Personalizada

### Interfaces a implementar
- `IValidator<T>`

### Registro personalizado
```csharp
services.AddTransient<IValidator<UpdateOrderCommand>, UpdateOrderValidator>();
```

### Ejemplo step-by-step
1. Crea el validador para la clase deseada
2. Registra en DI
3. Usa la validación en la capa de Application

---

## 7. Testing

### Mock/stub recommendations
- prueba los validadores directamente
- no mockees reglas simples

### Ejemplo de test
```csharp
[Fact]
public void CreateOrderValidator_Fails_WhenOrderNumberEmpty()
{
    var validator = new CreateOrderValidator();
    var result = validator.Validate(new CreateOrderCommand(string.Empty, new List<OrderLineCmd>()));

    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.PropertyName == "OrderNumber");
}
```