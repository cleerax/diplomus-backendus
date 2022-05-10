namespace DiplomusContractors.Products;

public interface IProductsService
{
    public Task<int> GetPageCountAsync(int pageNumber, CancellationToken cancellationToken);
    public IAsyncEnumerable<Product> GetProductsPageAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);
}
