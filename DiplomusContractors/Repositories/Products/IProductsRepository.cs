namespace DiplomusContractors.Repositories.Products;

public interface IProductsRepository
{
    public Task<int> GetPagesCountAsync(int pageSize, CancellationToken cancellationToken);
    public IAsyncEnumerable<Product> GetPageAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);
}
