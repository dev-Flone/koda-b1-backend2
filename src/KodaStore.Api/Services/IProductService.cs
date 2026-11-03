using KodaStore.Api.DTOs;

namespace KodaStore.Api.Services;

public interface IProductService
{
    IReadOnlyList<ProductResponse> GetAll();
    ProductResponse? GetByCode(string productCode);
    ProductResponse? Create(CreateProductRequest request);
    ProductResponse? Update(string productCode, UpdateProductRequest request);
    bool Delete(string productCode);

}