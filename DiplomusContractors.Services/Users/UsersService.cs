using DiplomusContractors.Repositories.Users;
using DiplomusContractors.Users;
using DiplomusContractors.Users.Contracts;
using DiplomusContractors.Users.Models;
using Microsoft.AspNetCore.Identity;

namespace DiplomusContractors.Services.Users;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _repository;
    private readonly IPasswordHasher<string> _passwordHasher;

    public UsersService(IUsersRepository repository, IPasswordHasher<string> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<User?> AuthenticateUserAsync(UserLoginRequest request, CancellationToken cancellationToken)
    {
        var userHashedPassword = await _repository.GetUserPasswordAsync(request.Username, cancellationToken);

        if (userHashedPassword is null)
            return null;

        if (_passwordHasher.VerifyHashedPassword(request.Username, userHashedPassword.HashedPassword, request.Password) == PasswordVerificationResult.Success)
            return await _repository.GetUserAsync(userHashedPassword.UserId, cancellationToken);
        else
            return null;
    }

    public async Task RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var hashedPassword = _passwordHasher.HashPassword(request.Username, request.Password);

        await _repository.AddUserAsync(request.Username, hashedPassword, request.Email, cancellationToken);
    }
}
