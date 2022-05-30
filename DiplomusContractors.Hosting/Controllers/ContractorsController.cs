using DiplomusContractors.Contractors;
using DiplomusContractors.Repositories;
using DiplomusContractors.Repositories.Contractors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiplomusContractors.Hosting.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ContractorsController : ControllerBase
{
    private readonly IContractorsRepository _contractorsRepository;

    public ContractorsController(IContractorsRepository contractorsRepository)
    {
        _contractorsRepository = contractorsRepository;
    }

    [HttpGet("pageCount")]
    [Authorize]
    public async Task<PageCountResponse> GetPageCountAsync([FromQuery] int pageSize = 30, CancellationToken cancellationToken = default) =>
        new PageCountResponse(await _contractorsRepository.GetPagesCountAsync(pageSize, cancellationToken));

    [HttpGet("{id}")]
    [Authorize]
    public async Task<Contractor?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await _contractorsRepository.GetByIdAsync(id, cancellationToken);

    [HttpGet]
    [Authorize]
    public async Task<Contractor[]> GetPageAsync([FromQuery] int pageSize = 30, [FromQuery] int pageNumber = 1, CancellationToken cancellationToken = default) =>
        await _contractorsRepository.GetPageAsync(pageSize, pageNumber, cancellationToken).ToArrayAsync(cancellationToken);

    [HttpGet("{id}/products")]
    [Authorize]
    public async Task<ContractorProduct[]> GetContractorProductsPageAsync(
        int id,
        [FromQuery] int pageSize = 30,
        [FromQuery] int pageNumber = 1,
        CancellationToken cancellationToken = default) =>
        await _contractorsRepository.GetContractorProductsPageAsync(id, pageSize, pageNumber, cancellationToken)
            .ToArrayAsync(cancellationToken);

    [HttpGet("{id}/products/pageCount")]
    [Authorize]
    public async Task<PageCountResponse> GetProuctsPageCountAsync(int id, [FromQuery] int pageSize = 30, CancellationToken cancellationToken = default) =>
        new PageCountResponse(await _contractorsRepository.GetProductPagesCountAsync(id, pageSize, cancellationToken));
}
