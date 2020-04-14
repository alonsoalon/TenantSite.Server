using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.HttpApi.SwaggerHelper;
using AlonsoAdmin.MultiTenant.Extensions;
using AlonsoAdmin.Services.System.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static AlonsoAdmin.HttpApi.SwaggerHelper.CustomApiVersion;

namespace AlonsoAdmin.HttpApi.Controllers.V1.System
{

    public class UserController : ModuleBaseController
    {
        
        private readonly ISysUserService _userServices;

        public UserController(ISysUserService userServices)
        {
            _userServices = userServices;
        }

        /// <summary>
        /// 得到单条用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> PageList(RequestEntity<SysUserEntity> req)
        {
            return await _userServices.GetPageAsync(req);
        }

        //[HttpGet]
        //public async Task<IResponseEntity> Item(long id)
        //{
        //    return null;
        //}


    }
}