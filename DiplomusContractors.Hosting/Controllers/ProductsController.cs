using DiplomusContractors.Products;
using DiplomusContractors.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomusContractors.Hosting.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _productsService;

    public ProductsController(IProductsService productsService)
    {
        _productsService = productsService;
    }

    [HttpGet("pageCount")]
    [Authorize]
    public async Task<PageCountResponse> GetPageCountAsync([FromQuery] int pageSize = 30, CancellationToken cancellationToken = default) =>
        new PageCountResponse(await _productsService.GetPageCountAsync(pageSize, cancellationToken));

    [HttpGet]
    [Authorize]
    public async Task<Product[]> GetProductsPageAsync([FromQuery] int pageSize = 30, [FromQuery] int pageNumber = 1, CancellationToken cancellationToken = default) =>
        await _productsService.GetProductsPageAsync(pageSize, pageNumber, cancellationToken).ToArrayAsync(cancellationToken);
}
