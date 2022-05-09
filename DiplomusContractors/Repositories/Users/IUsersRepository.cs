using DiplomusContractors.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomusContractors.Repositories.Users;

public interface IUsersRepository
{
    Task<UserHashedPassword?> GetUserPassword(string username, CancellationToken cancellationToken);
    Task<User?> GetUser(int userId, CancellationToken cancellationToken);
    Task AddUser(string username, string password, string? email, CancellationToken cancellationToken);
}
