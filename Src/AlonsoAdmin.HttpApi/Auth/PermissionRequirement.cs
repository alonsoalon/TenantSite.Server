using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlonsoAdmin.HttpApi.Auth
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public IEnumerable<string> AllowedRoles { get; set; }

        public PermissionRequirement() { }

        public PermissionRequirement(IEnumerable<string> allowedRoles)
        {
            AllowedRoles = allowedRoles;
        }
    }
}
