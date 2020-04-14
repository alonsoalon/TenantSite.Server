using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.Controllers
{


    [ApiController]
    [Authorize("default")]

    public abstract class BaseController : ControllerBase
    {

    }
 
}
