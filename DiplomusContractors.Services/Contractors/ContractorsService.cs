using DiplomusContractors.Contractors;
using DiplomusContractors.Repositories.Contractors;

namespace DiplomusContractors.Services.Contractors;

public class ContractorsService : IContractorsService
{
    private readonly IContractorsRepository _contractorsRepository;

    public ContractorsService(IContractorsRepository contractorsRepository)
    {
        _contractorsRepository = contractorsRepository;
    }

    public async Task<int> GetPageCountAsync(int pageSize, CancellationToken cancellationToken)
    {
        return await _contractorsRepository.GetPagesCountAsync(pageSize, cancellationToken);
    }

    public async Task<Contractor[]> GetPageAsync(int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        return await _contractorsRepository.GetPageAsync(pageSize, pageNumber, cancellationToken).ToArrayAsync(cancellationToken);
    }

    public async Task<Contractor?> GetByIdAsync(int contractorId, CancellationToken cancellationToken)
    {
        return await _contractorsRepository.GetByIdAsync(contractorId, cancellationToken);
    }

    public async Task<ContractorProduct[]> GetContractorProductsPageAsync(
        int contractorId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken)
    {
        return await _contractorsRepository.GetContractorProductsPageAsync(contractorId, pageSize, pageNumber, cancellationToken)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<int> GetProductsPageCountAsync(int contractorId, int pageSize, CancellationToken cancellationToken)
    {
        return await _contractorsRepository.GetProductPagesCountAsync(contractorId, pageSize, cancellationToken);
    }
}
