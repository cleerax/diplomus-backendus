using DiplomusContractors.Options.Repositories;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using DiplomusContractors.Users.Models;
using DiplomusContractors.Repositories.Users;

namespace DiplomusContractors.Persistance.Repositories;
public class UsersRepository : IUsersRepository
{
    private readonly string _connectionString;

    public UsersRepository(IOptions<DbOptions> options) => _connectionString = options.Value.ConnectionString;

    public async Task AddUserAsync(string username, string password, string? email, CancellationToken cancellationToken)
    {
        const string query = @"
INSERT INTO users (username, password, email) VALUES (@username, @password, @email);
SELECT LAST_INSERT_ID();";

        const string roleQuery = @"INSERT INTO user_roles (user_id) VALUES (@user_id);";

        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        using var command = new MySqlCommand(query, connection)
        {
            Parameters =
                {
                    new MySqlParameter("username", username),
                    new MySqlParameter("password", password),
                    new MySqlParameter("email", email)
                }
        };

        using var roleCommand = new MySqlCommand(roleQuery, connection);

        try
        {
            var userId = await command.ExecuteScalarAsync(cancellationToken);

            roleCommand.Parameters.Add(new MySqlParameter("user_id", userId));

            await roleCommand.ExecuteNonQueryAsync(cancellationToken);
        }
        catch (MySqlException ex)
        {
            if (ex.Number == 1062)
                throw new ArgumentException();
            else
                throw;
        }
    }

    public async Task<User?> GetUserAsync(int userId, CancellationToken cancellationToken)
    {
        const string rolesQuery = @"SELECT user_role FROM user_roles WHERE user_id = @user_id;";

        const string query = @"
SELECT user_id, username, email, registration_date
FROM users
WHERE user_id = @id;
";

        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        using var rolesCommand = new MySqlCommand(rolesQuery, connection)
        {
            Parameters =
            {
                new MySqlParameter("user_id", userId)
            }
        };

        var roles = new List<UserRole>();

        await using (var reader = await rolesCommand.ExecuteReaderAsync(cancellationToken))
            while (await reader.ReadAsync(cancellationToken))
                roles.Add(Enum.Parse<UserRole>(reader.GetString(0), true));

        using var command = new MySqlCommand(query, connection)
        {
            Parameters =
                {
                    new MySqlParameter("id", userId)
                }
        };

        await using (var reader = await command.ExecuteReaderAsync(cancellationToken))
            if (await reader.ReadAsync(cancellationToken))
            {
                return new User
                {
                    UserId = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    Email = reader.GetString(2),
                    Roles = roles.ToArray(),
                    RegistrationDate = reader.GetFieldValue<DateTimeOffset>(3)
                };
            }
            else
                return null;
    }

    public async Task<UserHashedPassword?> GetUserPasswordAsync(string username, CancellationToken cancellationToken)
    {
        const string query = @"
SELECT user_id, password
FROM users
WHERE username = @username
";

        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        using var command = new MySqlCommand(query, connection)
        {
            Parameters =
                {
                    new MySqlParameter("username", username)
                }
        };

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            return new UserHashedPassword(reader.GetInt32(0), reader.GetString(1));
        }
        else
            return null;
    }
}
