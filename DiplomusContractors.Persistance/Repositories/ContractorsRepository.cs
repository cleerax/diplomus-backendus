using System.Runtime.CompilerServices;
using DiplomusContractors.Options.Repositories;
using DiplomusContractors.Repositories.Contractors;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using DiplomusContractors.Contractors;
using static DiplomusContractors.Products.Product;
using DiplomusContractors.Products;

namespace DiplomusContractors.Persistance.Repositories;

public class ContractorsRepository : IContractorsRepository
{
    private readonly string _connectionString;

    public ContractorsRepository(IOptions<DbOptions> options) => _connectionString = options.Value.ConnectionString;

    public async Task<int> GetPagesCountAsync(int pageSize, CancellationToken cancellationToken)
    {
        const string query = @"SELECT CEILING(COUNT(*) / @page_size) FROM contractors;";

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

    public async Task<int> GetProductPagesCountAsync(int contractorId, int pageSize, CancellationToken cancellationToken)
    {
        const string query = @"
SELECT CEILING(COUNT(*) / @page_size)
FROM contractors_products JOIN products USING (product_id)
WHERE contractor_id = @contractor_id;
";

        await using var connection = new MySqlConnection(_connectionString);
        using var command = new MySqlCommand(query, connection)
        {
            Parameters =
            {
                new MySqlParameter("contractor_id", contractorId),
                new MySqlParameter("page_size", pageSize)
            }
        };

        await connection.OpenAsync(cancellationToken);
        var result = await command.ExecuteScalarAsync(cancellationToken);

        return (result is null || result == DBNull.Value) ? 0 : decimal.ToInt32((decimal)result);
    }

    public async IAsyncEnumerable<ProductContractor> GetProductContractorsAsync(int productId, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string query = @"
SELECT c.contractor_id,
       c.contractor_name,
       c.contractor_email,
       c.contractor_inn,
       c.contractor_address,
       c.image_link,
       cp.price
FROM contractors_products cp JOIN contractors c ON cp.contractor_id = c.contractor_id
WHERE cp.product_id = @product_id;
";

        await using var connection = new MySqlConnection(_connectionString);
        using var command = new MySqlCommand(query, connection)
        {
            Parameters =
            {
                new MySqlParameter("product_id", productId)
            }
        };

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new Products.Product.ProductContractor(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.IsDBNull(3) ? null : reader.GetString(3),
                reader.IsDBNull(4) ? null : reader.GetString(4),
                reader.IsDBNull(5) ? null : reader.GetString(5),
                reader.GetDecimal(6));
        }
    }

    public async IAsyncEnumerable<Contractor> GetPageAsync(int pageSize, int pageNumber, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string query = @"
SELECT contractor_id, contractor_name, contractor_email, contractor_inn, contractor_address, image_link, active_time
FROM contractors
LIMIT @skip, @limit;";

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
            yield return new Contractors.Contractor(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.IsDBNull(3) ? null : reader.GetString(3),
                reader.IsDBNull(4) ? null : reader.GetString(4),
                reader.IsDBNull(5) ? null : reader.GetString(5),
                reader.IsDBNull(6) ? null : reader.GetString(6));
        }
    }

    public async Task<Contractor?> GetByIdAsync(int contractorId, CancellationToken cancellationToken)
    {
        const string query = @"
SELECT contractor_id, contractor_name, contractor_email, contractor_inn, contractor_address, image_link, active_time
FROM contractors
WHERE contractor_id = @contractor_id;
";

        await using var connection = new MySqlConnection(_connectionString);
        using var command = new MySqlCommand(query, connection)
        {
            Parameters =
            {
                new MySqlParameter("contractor_id", contractorId)
            }
        };

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            return new Contractors.Contractor(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.IsDBNull(3) ? null : reader.GetString(3),
                reader.IsDBNull(4) ? null : reader.GetString(4),
                reader.IsDBNull(5) ? null : reader.GetString(5),
                reader.IsDBNull(6) ? null : reader.GetString(6));
        }

        return null;
    }

    public async IAsyncEnumerable<ContractorProduct> GetContractorProductsPageAsync(
        int contractorId,
        int pageSize,
        int pageNumber,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string query = @"
SELECT product_id, product_name, product_status, price
FROM contractors_products JOIN products USING (product_id)
WHERE contractor_id = @contractor_id
LIMIT @skip, @limit;
";

        await using var connection = new MySqlConnection(_connectionString);
        using var command = new MySqlCommand(query, connection)
        {
            Parameters =
            {
                new MySqlParameter("contractor_id", contractorId),
                new MySqlParameter("skip", pageSize * (pageNumber - 1)),
                new MySqlParameter("limit", pageSize)
            }
        };

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new ContractorProduct(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2).ConvertToProductStatus(),
                reader.GetDecimal(3));
        }
    }
}
