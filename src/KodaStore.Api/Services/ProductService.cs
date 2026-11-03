using KodaStore.Api.DTOs;
using KodaStore.Api.Models;
using KodaStore.Api.Repositories;

namespace KodaStore.Api.Services;

public sealed class ProductService(IProductRepository repository) : IProductService
{
    private readonly IProductRepository _repository = repository;

    public ProductResponse? Create(CreateProductRequest request)
    {
        var product = new Product(request.ProductCode, request.Name, request.Price, request.Stock, request.Category);

        return _repository.TryAdd(product) ? ProductResponse.FromModel(product) : null;
    }

    public bool Delete(string productCode)
    {
        return _repository.TryDelete(productCode);
    }

    public IReadOnlyList<ProductResponse> GetAll()
    {
        return [.. _repository.GetAll().Select(ProductResponse.FromModel)];
    }

    public ProductResponse? GetByCode(string productCode)
    {
        var product = _repository.GetByCode(productCode);
        return product is null ? null : ProductResponse.FromModel(product);
    }

    public ProductResponse? Update(string productCode, UpdateProductRequest request)
    {
        var product = new Product(productCode, request.Name, request.Price, request.Stock, request.Category);

        return _repository.TryUpdate(product) ? ProductResponse.FromModel(product) : null;
    }
}