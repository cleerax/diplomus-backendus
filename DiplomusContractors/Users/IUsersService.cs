using DiplomusContractors.Users.Contracts;
using DiplomusContractors.Users.Models;

namespace DiplomusContractors.Users;

public interface IUsersService
{
    Task<User?> AuthenticateUser(UserLoginRequest request, CancellationToken cancellationToken);
    Task RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken);
}
