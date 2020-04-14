using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace AlonsoAdmin.Common.Auth
{
    public interface IAuthToken
    {
        string Build(Claim[] claims);
    }
}
