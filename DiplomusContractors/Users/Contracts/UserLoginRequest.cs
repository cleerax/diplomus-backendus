using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomusContractors.Users.Contracts;

public record UserLoginRequest(string Username, string Password);
