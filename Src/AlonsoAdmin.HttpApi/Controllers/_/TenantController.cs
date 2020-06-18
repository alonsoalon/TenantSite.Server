using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlonsoAdmin.Common.Configs;
using AlonsoAdmin.Common.ResponseEntity;
using AlonsoAdmin.HttpApi.Attributes;
using AlonsoAdmin.MultiTenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlonsoAdmin.HttpApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TenantController : ControllerBase
    {
        private readonly IOptionsMonitor<List<TenantInfo>> _tenants;

        public TenantController(IOptionsMonitor<List<TenantInfo>> tenants)
        {
            _tenants = tenants;
        }

        // POST api/<controller>
        [HttpGet]
        [AllowAnonymous]
        [NoOprationLog]
        public IResponseEntity GetList()
        {
            var tenants = _tenants.CurrentValue;
            ArrayList data = new ArrayList();
            foreach (var tenant in tenants.OrderBy(x=>x.Id)) {
                data.Add(new { id = tenant.Id, code = tenant.Code, name = tenant.Name });
            }
            return ResponseEntity.Ok(data);
        }

    }
}
