using DiplomusContractors.Users.Contracts;
using DiplomusContractors.Users.Models;

namespace DiplomusContractors.Users;

public interface IUsersService
{
    Task<User?> AuthenticateUserAsync(UserLoginRequest request, CancellationToken cancellationToken);
    Task RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken);
}
