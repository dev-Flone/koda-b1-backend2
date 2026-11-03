using KodaStore.Api.Models;
using KodaStore.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using KodaStore.Api.Services;

namespace KodaStore.Api.Controllers;

[ApiController]
[Route("api/v1/products")]

public sealed class ProductController(IProductService service) : ControllerBase
{
    private readonly IProductService _service = service;

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<ProductResponse>> GetAll()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("{productCode}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<ProductResponse> GetByCode([FromRoute] string productCode)
    {
        var product = _service.GetByCode(productCode);

        return product is null ? Problem(statusCode: StatusCodes.Status404NotFound, title: "Product not found") : Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<ProductResponse> Create([FromBody] CreateProductRequest request)
    {
        var product = _service.Create(request);

        if (product is null)
        {
            return Problem(statusCode: StatusCodes.Status409Conflict, title: "Product code already exist.");
        }

        return CreatedAtAction(nameof(GetByCode), new { productCode = product.ProductCode }, product);
    }

    [HttpDelete("{productCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult Delete([FromRoute] string productCode)
    {
        return _service.Delete(productCode) ? NoContent() : Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Can't find product.",
                detail: $"There is no product with code {productCode}"
            );
    }

    [HttpPut("{productCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<ProductResponse> Update([FromRoute] string productCode, [FromBody] UpdateProductRequest request)
    {
        try
        {
            var existingProduct = _service.Update(productCode, request);

            return existingProduct is null ? Problem(statusCode: StatusCodes.Status404NotFound, title: "Product Not Found.", detail: $"No product was found for code {productCode}") : Ok(existingProduct);
        }
        catch (ArgumentException e)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Invalid product", detail: e.Message);
        }
    }
}