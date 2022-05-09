using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomusContractors.Users.Models;

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole[] Roles { get; set; } = Array.Empty<UserRole>();
    public DateTimeOffset RegistrationDate { get; set; } = DateTimeOffset.UtcNow;
}
