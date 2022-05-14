namespace DiplomusContractors.Contractors;

public interface IContractorsService
{
    public Task<int> GetPageCountAsync(int pageSize, CancellationToken cancellationToken);
    public Task<Contractor[]> GetPageAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);
    public Task<Contractor?> GetByIdAsync(int contractorId, CancellationToken cancellationToken);
    public Task<ContractorProduct[]> GetContractorProductsPageAsync(
        int contractorId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken);
    public Task<int> GetProductsPageCountAsync(int contractorId, int pageSize, CancellationToken cancellationToken);
}
