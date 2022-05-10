using DiplomusContractors.Options.Repositories;
using DiplomusContractors.Products;
using DiplomusContractors.Repositories.Products;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Runtime.CompilerServices;
using Product = DiplomusContractors.Repositories.Products.Product;

namespace DiplomusContractors.Persistance.Repositories;

public class ProductsRepository : IProductsRepository
{
    private readonly string _connectionString;

    public ProductsRepository(IOptions<DbOptions> options) => _connectionString = options.Value.ConnectionString;

    public async Task<int> GetPagesCountAsync(int pageSize, CancellationToken cancellationToken)
    {
        const string query = @"SELECT CEILING(COUNT(*) / @page_size) FROM products;";

        await using var connection = new MySqlConnection(_connectionString);
        using var command = new MySqlCommand(query, connection)
        {
            Parameters =
            {
                new MySqlParameter("page_size", pageSize)
            }
        };

        await connection.OpenAsync(cancellationToken);
        var result = await command.ExecuteScalarAsync(cancellationToken);

        return (result is null || result == DBNull.Value) ? 0 : decimal.ToInt32((decimal)result);
    }

    public async IAsyncEnumerable<Product> GetPageAsync(int pageSize, int pageNumber, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string query = @"
SELECT product_id, product_name, product_status
FROM products
LIMIT @skip, @limit";

        await using var connection = new MySqlConnection(_connectionString);
        using var command = new MySqlCommand(query, connection)
        {
            Parameters =
            {
                new MySqlParameter("skip", pageSize * (pageNumber - 1)),
                new MySqlParameter("limit", pageSize)
            }
        };

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new Product(reader.GetInt32(0), reader.GetString(1), reader.GetString(2).ConvertToProductStatus());
        }
    }
}
