using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomusContractors.Users.Contracts;

public record class RegisterUserRequest(string Username, string Password, string Email);
