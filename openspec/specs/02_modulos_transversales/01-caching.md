# 01 - Caching (Hybrid + Redis)

**Sección**: Módulos Transversales | **Documento**: 01 de 10 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Proveer una capa de caché transparente que soporte:
- caché en memoria para respuestas rápidas
- cache distribuida en Redis para escalabilidad
- refresco y expiración configurables

### Casos de uso
- resultados de consultas frecuentes
- catálogos de referencia
- datos de configuración que cambian ocasionalmente
- evitar cargas repetidas en la base de datos

### Stack tecnológico
- `Microsoft.Extensions.Caching.Memory`
- `StackExchange.Redis`
- `Microsoft.Extensions.Caching.Distributed`

---

## 2. Abstracciones Principales

### Interfaces clave
- `ICacheProvider`
- `IHybridCache`
- `ICacheKeyGenerator`

### Clases abstractas
- `CacheProviderBase`
- `HybridCacheBase`

### Namespaces
- `CompanyName.Caching`
- `CompanyName.Caching.Redis`

---

## 3. Comportamiento Default

### Implementación out-of-box
- `MemoryCacheProvider`: cache en memoria local
- `RedisCacheProvider`: cache distribuida basado en Redis
- `HybridCache`: combina memoria + Redis
- `CacheKeyBuilder`: genera llaves consistentes

### Reglas predeterminadas
- TTL por entrada configurable
- `CacheAside` como patrón principal
- serialización JSON para objetos complejos

---

## 4. Configuración en Program.cs

```csharp
public static WebApplicationBuilder CreateBuilder(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCaching(options =>
    {
        options.UseInMemoryCache = true;
        options.RedisConfiguration = builder.Configuration.GetConnectionString("Redis");
        options.DefaultSlidingExpiration = TimeSpan.FromMinutes(5);
    });

    return builder;
}
```

```csharp
public static class CachingServiceCollectionExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services, Action<CachingOptions> configure)
    {
        var options = new CachingOptions();
        configure(options);

        services.AddSingleton(options);
        services.AddMemoryCache();

        if (!string.IsNullOrWhiteSpace(options.RedisConfiguration))
        {
            services.AddStackExchangeRedisCache(redis =>
            {
                redis.Configuration = options.RedisConfiguration;
            });
            services.AddSingleton<ICacheProvider, RedisCacheProvider>();
            services.AddSingleton<IHybridCache, HybridCache>();
        }
        else
        {
            services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
        }

        services.AddSingleton<ICacheKeyGenerator, CacheKeyBuilder>();
        return services;
    }
}
```

---

## 5. Patrones de Uso

### Inyección de dependencias
```csharp
public class ProductService
{
    private readonly ICacheProvider _cache;
    private readonly IProductRepository _repository;

    public ProductService(ICacheProvider cache, IProductRepository repository)
    {
        _cache = cache;
        _repository = repository;
    }
}
```

### Uso práctico
```csharp
var cacheKey = _cacheKeyGenerator.Create("Product", productId);
var product = await _cache.GetAsync<Product>(cacheKey);
if (product is null)
{
    product = await _repository.GetByIdAsync(productId);
    await _cache.SetAsync(cacheKey, product, TimeSpan.FromMinutes(10));
}
```

### Best practices
- Cachear resultados de consultas costosas
- Incluir versión/namespace en la llave
- No cachear datos sensibles sin cifrado
- Expirar en períodos cortos para datos muy dinámicos

---

## 6. Extensión Personalizada

### Interfaces a implementar
- `ICacheProvider`
- `IHybridCache`
- `ICacheKeyGenerator`

### Registro de implementación personalizada
```csharp
services.AddSingleton<ICacheProvider, CustomRedisCacheProvider>();
services.AddSingleton<IHybridCache, CustomHybridCache>();
```

### Ejemplo step-by-step
1. Implementa `ICacheProvider`
2. Registra tu clase en DI
3. Usa la interfaz desde tus servicios

---

## 7. Testing

### Mock/stub recommendations
- Mock `ICacheProvider`
- Simula `GetAsync` / `SetAsync`

### Ejemplo de test
```csharp
[Fact]
public async Task GetProduct_CachesResult()
{
    var cacheMock = new Mock<ICacheProvider>();
    cacheMock.Setup(c => c.GetAsync<Product>(It.IsAny<string>()))
             .ReturnsAsync((Product?)null);

    var repositoryMock = new Mock<IProductRepository>();
    repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Product { Id = 1 });

    var service = new ProductService(cacheMock.Object, repositoryMock.Object);
    await service.GetProductAsync(1);

    cacheMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<Product>(), It.IsAny<TimeSpan>()), Times.Once);
}
```
