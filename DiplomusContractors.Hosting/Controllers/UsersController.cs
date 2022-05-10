using DiplomusContractors.Hosting.Authorization;
using DiplomusContractors.Users;
using DiplomusContractors.Users.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DiplomusContractors.Hosting.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _usersService.RegisterUserAsync(request, cancellationToken);
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPost("login")]
    public async Task<UserLoginResponse?> LoginAsync([FromBody] UserLoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _usersService.AuthenticateUserAsync(request, cancellationToken);

        if (user == null)
            return null;

        var claimsIdentity = new ClaimsIdentity(
            user.Roles.Select(c => new Claim(ClaimsIdentity.DefaultRoleClaimType, c.ToString())),
            "Token",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: claimsIdentity.Claims,
            expires: now.Add(TimeSpan.FromDays(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new UserLoginResponse(user, encodedJwt);
    }

    [HttpGet("test")]
    [Authorize(Roles = "Admin")]
    public async Task TestCall()
    {
        await Task.CompletedTask;
    }
}
