using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlonsoAdmin.Entities;
using AlonsoAdmin.Entities.System;
using AlonsoAdmin.Services.System.Interface;
using AlonsoAdmin.Services.System.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlonsoAdmin.HttpApi.Controllers.V1.System
{

    public class GroupController : ModuleBaseController
    {
        private readonly ISysGroupService _sysGroupService;
        public GroupController(ISysGroupService sysGroupService)
        {
            _sysGroupService = sysGroupService;
        }

        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> Create(GroupAddRequest req)
        {
            return await _sysGroupService.CreateAsync(req);
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseEntity> Update(GroupEditRequest req)
        {
            return await _sysGroupService.UpdateAsync(req);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> Delete(string id)
        {
            return await _sysGroupService.DeleteAsync(id);
        }

        /// <summary>
        /// 软删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseEntity> SoftDelete(string id)
        {
    

            return await _sysGroupService.SoftDeleteAsync(id);
        }


        [HttpPost]
        public IResponseEntity BatchSoftDelete(string[] ids)
        {
            return ResponseEntity.Error("nononono");
        }

        /// <summary>
        /// 取得分页列表数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetList(RequestEntity<GroupFilterRequest> req)
        {
            return await _sysGroupService.GetListAsync(req);
        }

        /// <summary>
        /// 取得所有的数据（不分页）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseEntity> GetAll(GroupFilterRequest req)
        {
            return await _sysGroupService.GetAllAsync(req);
        }
    }
}