using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DiplomusContractors.Hosting.Authorization;

public static class AuthOptions
{
    public const string ISSUER = "ContractorsServer";
    public const string AUDIENCE = "ContractorsClient";
    const string KEY = "mysupersecret_secretkey!123";
    public const int LIFETIME = 1;
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}
