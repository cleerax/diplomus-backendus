using System.Runtime.CompilerServices;
using DiplomusContractors.Options.Repositories;
using DiplomusContractors.Repositories.Contractors;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using static DiplomusContractors.Products.Product;

namespace DiplomusContractors.Persistance.Repositories;

public class ContractorsRepository : IContractorsRepository
{
    private readonly string _connectionString;

    public ContractorsRepository(IOptions<DbOptions> options) => _connectionString = options.Value.ConnectionString;

    public Task<int> GetPagesCount(int pageSize, CancellationToken cancellation)
    {
        const string query = @"";

        throw new NotImplementedException();
    }

    public async IAsyncEnumerable<Contractor> GetProductContractors(int productId, [EnumeratorCancellation] CancellationToken cancellationToken)
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
            yield return new Contractor(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.IsDBNull(3) ? null : reader.GetString(3),
                reader.IsDBNull(4) ? null : reader.GetString(4),
                reader.IsDBNull(5) ? null : reader.GetString(5),
                reader.GetDecimal(6));
        }
    }
}
