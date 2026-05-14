# 07 - Security & Claims

**Sección**: Módulos Transversales | **Documento**: 07 de 10 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Proveer una capa de seguridad básica para:
- identificación de usuario
- autorización basada en claims y roles
- acceso a recursos sensible

### Casos de uso
- restringir endpoints por roles
- obtener el usuario actual en servicios
- validar permisos en operaciones de negocio

### Stack tecnológico
- `System.Security.Claims`
- `Microsoft.AspNetCore.Authentication`
- `Microsoft.AspNetCore.Authorization`
- `CompanyName.Security`

---

## 2. Abstracciones Principales

### Interfaces clave
- `ICurrentUser`
- `IAuthorizationService`
- `IPermissionChecker`

### Clases base
- `CurrentUserAccessor`
- `ClaimsPrincipalExtensions`
- `PermissionRequirement`

### Namespaces
- `CompanyName.Security`
- `CompanyName.Security.Authorization`

---

## 3. Comportamiento Default

### Implementación out-of-box
- `CurrentUserAccessor` lee claims desde `HttpContext`
- `PermissionRequirement` verifica permisos
- `ClaimsPrincipalExtensions` extrae roles y user id

### Reglas predeterminadas
- el usuario actual está disponible como `ICurrentUser`
- las autorizaciones usan claims estándar (`sub`, `role`)
- el framework provee middleware para validación de tokens

---

## 4. Configuración en Program.cs

```csharp
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth:Authority"];
        options.Audience = builder.Configuration["Auth:Audience"];
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddScoped<ICurrentUser, CurrentUserAccessor>();
```

---

## 5. Patrones de Uso

### Inyección de dependencias
```csharp
public class CreateOrderService : ApplicationService
{
    private readonly ICurrentUser _currentUser;

    public CreateOrderService(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }
}
```

### Uso práctico
```csharp
[Authorize(Roles = "Admin")]
public class OrdersController : CompanyNameController
{
    public async Task<IActionResult> Delete(int id)
    {
        // Solo usuarios con rol Admin
    }
}
```

### Best practices
- usa claims en lugar de roles cuando necesites granularidad
- no confíes en datos del token sin validación
- aplica autorización en la capa de Presentation y Domain

---

## 6. Extensión Personalizada

### Interfaces a implementar
- `ICurrentUser`
- `IPermissionChecker`

### Registro personalizado
```csharp
services.AddScoped<ICurrentUser, CustomCurrentUserAccessor>();
services.AddScoped<IPermissionChecker, CustomPermissionChecker>();
```

### Ejemplo step-by-step
1. Implementa `ICurrentUser` para leer claims de JWT u otro token
2. Implementa `IPermissionChecker` según tu modelo de permisos
3. Registra en DI
4. Usa `CheckPermission` desde Application Services

---

## 7. Testing

### Mock/stub recommendations
- Mock `ICurrentUser`
- Simula claims en `ClaimsPrincipal`

### Ejemplo de test
```csharp
[Fact]
public void CurrentUserAccessor_ReadsUserIdFromClaims()
{
    var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] {
        new Claim("sub", "user-1")
    }, "jwt"));

    var context = new DefaultHttpContext { User = principal };
    var accessor = new CurrentUserAccessor(new HttpContextAccessor { HttpContext = context });

    Assert.Equal("user-1", accessor.Id);
}
```