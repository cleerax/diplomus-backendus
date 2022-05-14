using DiplomusContractors.Products;
using DiplomusContractors.Repositories.Contractors;
using DiplomusContractors.Repositories.Products;
using System.Runtime.CompilerServices;

namespace DiplomusContractors.Services.Products;

public class ProductsService : IProductsService
{
    private readonly IProductsRepository _productsRepository;
    private readonly IContractorsRepository _contractorRepository;

    public ProductsService(IProductsRepository repository, IContractorsRepository contractorsRepository)
    {
        _productsRepository = repository;
        _contractorRepository = contractorsRepository;
    }

    public async Task<int> GetPageCountAsync(int pageSize, CancellationToken cancellationToken)
    {
        return await _productsRepository.GetPagesCountAsync(pageSize, cancellationToken);
    }

    public async IAsyncEnumerable<DiplomusContractors.Products.Product> GetProductsPageAsync(int pageSize, int pageNumber, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var product in _productsRepository.GetPageAsync(pageSize, pageNumber, cancellationToken))
        {
            yield return new DiplomusContractors.Products.Product(
                product.ProductId,
                product.ProductName,
                product.ProductStatus,
                await _contractorRepository.GetProductContractorsAsync(product.ProductId, cancellationToken).ToArrayAsync(cancellationToken));
        }
    }
}
