using DiplomusContractors.Users.Models;

namespace DiplomusContractors.Users.Contracts;

public record UserLoginResponse(User User, string token);
