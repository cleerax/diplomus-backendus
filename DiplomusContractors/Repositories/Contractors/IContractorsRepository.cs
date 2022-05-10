using static DiplomusContractors.Products.Product;

namespace DiplomusContractors.Repositories.Contractors;

public interface IContractorsRepository
{
    public Task<int> GetPagesCount(int pageSize, CancellationToken cancellation);
    public IAsyncEnumerable<Contractor> GetProductContractors(int productId, CancellationToken cancellationToken);
}
