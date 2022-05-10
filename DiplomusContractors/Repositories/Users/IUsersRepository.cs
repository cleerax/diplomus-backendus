using DiplomusContractors.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomusContractors.Repositories.Users;

public interface IUsersRepository
{
    Task<UserHashedPassword?> GetUserPasswordAsync(string username, CancellationToken cancellationToken);
    Task<User?> GetUserAsync(int userId, CancellationToken cancellationToken);
    Task AddUserAsync(string username, string password, string? email, CancellationToken cancellationToken);
}
