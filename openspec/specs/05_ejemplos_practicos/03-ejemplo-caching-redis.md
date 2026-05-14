# 03 - Caching + Redis + Invalidation

**Sección**: Ejemplos Prácticos | **Documento**: 03 de 04 | **Estado**: v1.0.0-rc

---

## 1. Overview

### Propósito
Mostrar cómo usar caché distribuida con Redis y cómo invalidar entradas cuando los datos cambian.

### Casos de uso
- cachear listados frecuentes
- reducir carga en la base de datos
- invalidar caché tras actualizaciones

---

## 2. Configuración de caching

### Program.cs
```csharp
builder.Services.AddCaching(options =>
{
    options.UseInMemoryCache = true;
    options.RedisConfiguration = builder.Configuration.GetConnectionString("Redis");
    options.DefaultSlidingExpiration = TimeSpan.FromMinutes(10);
});
```

---

## 3. Uso del cache provider

### Servicio de catálogo
```csharp
public class ProductService
{
    private readonly ICacheProvider _cache;
    private readonly IProductRepository _repository;
    private readonly ICacheKeyGenerator _keyGenerator;

    public ProductService(ICacheProvider cache, IProductRepository repository, ICacheKeyGenerator keyGenerator)
    {
        _cache = cache;
        _repository = repository;
        _keyGenerator = keyGenerator;
    }

    public async Task<ProductDto> GetProductAsync(int id)
    {
        var key = _keyGenerator.Create("Product", id);
        var product = await _cache.GetAsync<ProductDto>(key);

        if (product != null)
            return product;

        product = await _repository.GetByIdAsync(id);
        await _cache.SetAsync(key, product, TimeSpan.FromMinutes(15));

        return product;
    }
}
```

---

## 4. Invalidación de caché

### Actualización de producto
```csharp
public async Task UpdateProductAsync(ProductUpdateCommand command)
{
    var product = await _repository.GetByIdAsync(command.Id);
    product.UpdateName(command.Name);
    await _repository.UpdateAsync(product);

    var key = _keyGenerator.Create("Product", product.Id);
    await _cache.RemoveAsync(key);
}
```

### Invalidación en cascade
```csharp
await _cache.RemoveAsync(_keyGenerator.Create("ProductList", "All"));
```

---

## 5. Best practices

- no cachear datos sensibles sin cifrado
- usa keys con namespace y versión
- invalidar caché tras cada modificación relevante
- evita cachear objetos que cambian frecuentemente

---

## 6. Testing

### Mock cache
```csharp
[Fact]
public async Task GetProductAsync_CachesResult()
{
    var cacheMock = new Mock<ICacheProvider>();
    cacheMock.Setup(c => c.GetAsync<ProductDto>(It.IsAny<string>())).ReturnsAsync((ProductDto?)null);

    var repoMock = new Mock<IProductRepository>();
    repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ProductDto { Id = 1, Name = "Test" });

    var service = new ProductService(cacheMock.Object, repoMock.Object, new CacheKeyGenerator());
    var result = await service.GetProductAsync(1);

    cacheMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<ProductDto>(), It.IsAny<TimeSpan>()), Times.Once);
}
```

---

## 7. Checklist

- [ ] Caching configurado
- [ ] Cache provider usado en el servicio
- [ ] Redis configurado en `Program.cs`
- [ ] Invalidación implementada
- [ ] Test del cache creado
