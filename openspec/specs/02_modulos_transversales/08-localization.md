# 08 - Localization

**Sección**: Módulos Transversales | **Documento**: 08 de 10 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Permitir aplicaciones multilíngües y adaptables a distintos mercados mediante:
- recursos localizados para UI y mensajes
- selección de cultura automática
- fallback y soporte de traducciones

### Casos de uso
- aplicaciones globales con varios idiomas
- mensajes de error localizados
- interfaces multi-región

### Stack tecnológico
- `Microsoft.Extensions.Localization`
- `Microsoft.AspNetCore.Localization`
- archivos `.resx`
- `CompanyName.Localization`

---

## 2. Abstracciones Principales

### Interfaces clave
- `IStringLocalizerFactory`
- `ILocalizationProvider`
- `IResourcePathResolver`

### Clases base
- `LocalizationService`
- `ResourceManagerStringLocalizerFactory`

### Namespaces
- `CompanyName.Localization`
- `CompanyName.Localization.Resources`

---

## 3. Comportamiento Default

### Implementación out-of-box
- `LocalizationService` expone `GetString(key)`
- middleware de ASP.NET Core ajusta la cultura actual
- soporte de `resx` para recursos compartidos

### Reglas predeterminadas
- si no existe traducción, reintenta con la cultura por defecto
- las claves usan convención `ResourceName.Key`
- los recursos se agrupan por feature/module

---

## 4. Configuración en Program.cs

```csharp
var supportedCultures = new[] { "en-US", "es-ES" };

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
});

var app = builder.Build();
app.UseRequestLocalization();
```

---

## 5. Patrones de Uso

### Inyección de dependencias
```csharp
public class GreetingService
{
    private readonly IStringLocalizer<GreetingService> _localizer;

    public GreetingService(IStringLocalizer<GreetingService> localizer)
    {
        _localizer = localizer;
    }

    public string GetWelcomeMessage() => _localizer["WelcomeMessage"];
}
```

### Uso práctico
```csharp
var message = _localizer["WelcomeMessage"];
```

### Best practices
- usa claves significativas en lugar de textos literales
- mantén los recursos por módulo para evitar duplicación
- añade comentarios de contexto en `.resx`

---

## 6. Extensión Personalizada

### Interfaces a implementar
- `ILocalizationProvider`
- `IResourcePathResolver`

### Registro personalizado
```csharp
services.AddSingleton<ILocalizationProvider, CustomLocalizationProvider>();
```

### Ejemplo step-by-step
1. Implementa un provider para cargar recursos desde BD o CMS
2. Registra el provider en DI
3. Usa `_localizer` en servicios y endpoints

---

## 7. Testing

### Mock/stub recommendations
- Mock `IStringLocalizer<T>`
- Probar fallback de cultura

### Ejemplo de test
```csharp
[Fact]
public void Localizer_ReturnsResourceValue()
{
    var localizerMock = new Mock<IStringLocalizer<GreetingService>>();
    localizerMock.Setup(x => x["WelcomeMessage"]).Returns(new LocalizedString("WelcomeMessage", "Bienvenido"));

    var service = new GreetingService(localizerMock.Object);
    Assert.Equal("Bienvenido", service.GetWelcomeMessage());
}
```