using DiplomusContractors.Contractors;
using DiplomusContractors.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomusContractors.Hosting.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ContractorsController : ControllerBase
{
    public readonly IContractorsService _contractorsService;

    public ContractorsController(IContractorsService contractorsService)
    {
        _contractorsService = contractorsService;
    }

    [HttpGet("pageCount")]
    [Authorize]
    public async Task<PageCountResponse> GetPageCountAsync([FromQuery] int pageSize = 30, CancellationToken cancellationToken = default) =>
        new PageCountResponse(await _contractorsService.GetPageCountAsync(pageSize, cancellationToken));

    [HttpGet("{id}")]
    [Authorize]
    public async Task<Contractor?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await _contractorsService.GetByIdAsync(id, cancellationToken);

    [HttpGet]
    [Authorize]
    public async Task<Contractor[]> GetPageAsync([FromQuery] int pageSize = 30, [FromQuery] int pageNumber = 1, CancellationToken cancellationToken = default) =>
        await _contractorsService.GetPageAsync(pageSize, pageNumber, cancellationToken);

    [HttpGet("{id}/products")]
    [Authorize]
    public async Task<ContractorProduct[]> GetContractorProductsPageAsync(
        int id,
        [FromQuery] int pageSize = 30,
        [FromQuery] int pageNumber = 1,
        CancellationToken cancellationToken = default) =>
        await _contractorsService.GetContractorProductsPageAsync(id, pageSize, pageNumber, cancellationToken);

    [HttpGet("{id}/products/pageCount")]
    [Authorize]
    public async Task<PageCountResponse> GetProuctsPageCountAsync(int id, [FromQuery] int pageSize = 30, CancellationToken cancellationToken = default) =>
        new PageCountResponse(await _contractorsService.GetProductsPageCountAsync(id, pageSize, cancellationToken));
}
