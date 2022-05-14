using static DiplomusContractors.Products.Product;
using DiplomusContractors.Contractors;

namespace DiplomusContractors.Repositories.Contractors;

public interface IContractorsRepository
{
    public Task<int> GetPagesCountAsync(int pageSize, CancellationToken cancellation);
    public IAsyncEnumerable<ProductContractor> GetProductContractorsAsync(int productId, CancellationToken cancellationToken);
    public IAsyncEnumerable<Contractor> GetPageAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);
    public Task<Contractor?> GetByIdAsync(int contractorId, CancellationToken cancellationToken);
    public IAsyncEnumerable<ContractorProduct> GetContractorProductsPageAsync(
        int contractorId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken);
    public Task<int> GetProductPagesCountAsync(int contractorId, int pageSize, CancellationToken cancellationToken);
}
